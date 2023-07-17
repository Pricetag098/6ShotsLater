using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wavemanager : MonoBehaviour
{
    public Transform player;
    public Health playerHealth;
    //public int waveNo = 0;
    int waveIndex = 0;
    int kills = 0;
    [Min(1)]
    public int waveChangeInterval = 1;
    SpawnPoint[] spawnPoints;

    Dictionary<Wave.SpawnRequest.ZombieTypes,ObjectPooler> zombiePools = new Dictionary<Wave.SpawnRequest.ZombieTypes, ObjectPooler> ();
    [SerializeField]
    List<Wave> waves = new List<Wave> ();

    List<ZombieAi> currentWave = new List<ZombieAi>();
    
    float spawnDelayTimer;
    [SerializeField] float SpawnDelay;

    [SerializeField] Optional<ScoreUI> scoreUI;
    public enum States
    {
        awaitingStart,
        awaitingSpawn,
        spawning,
        awaitingEndOfRound,
        dead
    }
    public States state;

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

    [ContextMenu("StartGame")]
    public void StartGame()
	{
        if(state == States.awaitingStart)
		{
            state = States.awaitingSpawn;
		}
	}

    [ContextMenu("KillAll")]
    public void KillAll()
    {
        foreach (ZombieAi zombie in currentWave )
        {
            zombie.KillInstant();
        }
    }

    int requestIndex = 0;
    
    float spawnTimer = 0;
    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case States.awaitingStart:
                //Do Nothing yet

                break;
            case States.awaitingSpawn:
                spawnDelayTimer += Time.deltaTime;
                if (spawnDelayTimer > SpawnDelay)
                {
                    //SpawnWave(waves[Mathf.Clamp(waveIndex, 0, waves.Count - 1)]);
                    state = States.spawning;
                    spawnDelayTimer = 0f;
                    requestIndex = 0;
                    spawnTimer = 0;
                }
                break;
            case States.spawning:
                spawnTimer += Time.deltaTime;
                Wave.SpawnRequest request = waves[Mathf.Clamp(waveIndex, 0, waves.Count-1)].spawnList[requestIndex];
                if (spawnTimer > request.spawnDelay)
                {
                    SpawnRequest(request);
                    spawnTimer = 0f;
                    requestIndex++;
                }
                if (requestIndex > waves[Mathf.Clamp(waveIndex, 0, waves.Count)].spawnList.Count - 1)
                {
                    state = States.awaitingEndOfRound;
                }


                break;
            case States.awaitingEndOfRound:
                bool alldead = true;
                for (int i = 0; i < currentWave.Count; i++)
                {
                    if (!currentWave[i].dead)
                    {
                        alldead = false;
                    }
                }
                if (alldead)
                {
                    waveIndex++;
                    currentWave.Clear();
                    state = States.awaitingSpawn;
                    if(scoreUI.Enabled)
                    scoreUI.Value.UpdateBoard(kills, waveIndex);
                }
                break;

            case States.dead:
                if(currentWave.Count > 0)
				{
                    for(int i = 0; i < currentWave.Count; i++)
					{
                        currentWave[i].KillInstant();
					}
                    currentWave.Clear();
				}

                break;
        }

    }

    void SpawnRequest(Wave.SpawnRequest request)
	{
        for (int i = 0; i < request.number; i++)
        {

            float randVal = Random.Range(request.moveSpeedMin,request.moveSpeedMax);
            SpawnZombie(request.zombieType,randVal);
        }
    }

    void SpawnZombie(Wave.SpawnRequest.ZombieTypes type,float speed)
    {
        GameObject zomGo = zombiePools[type].Spawn();
        SpawnPoint spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        zomGo.transform.position = spawnPoint.transform.position;
        //TODO add radius spawn

        ZombieAi zombieAi = zomGo.GetComponent<ZombieAi>();
        zombieAi.Spawn(player,playerHealth,speed);
        currentWave.Add(zombieAi);
        zombieAi.wavemanager = this;
    }

    public void AddKill()
    {
        kills++;
        if (scoreUI.Enabled)
            scoreUI.Value.UpdateBoard(kills, waveIndex);
    }


}
