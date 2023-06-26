using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavemanager : MonoBehaviour
{
    public Transform player;
    public Health playerHealth;
    public int waveNo = 0;
    int waveIndex = 0;
    [Min(1)]
    public int waveChangeInterval = 1;
    SpawnPoint[] spawnPoints;

    Dictionary<Wave.SpawnRequest.ZombieTypes,ObjectPooler> zombiePools = new Dictionary<Wave.SpawnRequest.ZombieTypes, ObjectPooler> ();
    [SerializeField]
    List<Wave> waves = new List<Wave> ();

    List<ZombieAi> currentWave = new List<ZombieAi>();
    bool awaitingSpawn = true;
    float timer;
    [SerializeField] float SpawnDelay;
    // Start is called before the first frame update
    void Start()
    {
        spawnPoints = FindObjectsOfType<SpawnPoint>();
        for(int i = 0; i < Wave.zombieTypeCount; i++)
        {
            zombiePools[(Wave.SpawnRequest.ZombieTypes)i] = transform.GetChild(i).GetComponent<ObjectPooler>();
        }
        
        //SpawnWave(waves[Mathf.Clamp(waveIndex, 0, waves.Count)]);
    }

    // Update is called once per frame
    void Update()
    {
        if (awaitingSpawn)
        {
            timer += Time.deltaTime;
            if(timer > SpawnDelay)
            {
                SpawnWave(waves[Mathf.Clamp(waveIndex, 0, waves.Count - 1)]);
                awaitingSpawn = false;
                timer = 0f;
            }
        }
        else
        {
            bool allDead = true;
            for (int i = 0; i < currentWave.Count; i++)
            {
                if (currentWave[i].gameObject.activeSelf)
                {
                    allDead = false;
                }
            }
            if (allDead)
            {
                waveNo++;
                currentWave.Clear();
                if (waveNo % waveChangeInterval == 0)
                    waveIndex++;
                awaitingSpawn = true;
            }
        }
        
    }
    [ContextMenu("TEST")]
    void TestSpawn1()
    {
        SpawnWave(waves[0]);
    }
    void SpawnWave(Wave wave)
    {
        for(int requestIndex = 0; requestIndex < wave.spawnList.Count; requestIndex++)
        {
            for(int i = 0; i < wave.spawnList[requestIndex].num; i++)
            {
                SpawnZombie(wave.spawnList[requestIndex].zombieType);
            }
        }
    }

    void SpawnZombie(Wave.SpawnRequest.ZombieTypes type)
    {
        GameObject zomGo = zombiePools[type].Spawn();
        SpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        zomGo.transform.position = spawnPoint.transform.position;
        //TODO add radius spawn

        ZombieAi zombieAi = zomGo.GetComponent<ZombieAi>();
        zombieAi.Spawn(player,playerHealth);
        currentWave.Add(zombieAi);
    }


}
