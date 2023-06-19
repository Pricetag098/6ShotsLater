using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A general purpose class for spawning 
/// </summary>
public class ObjectPooler : MonoBehaviour
{

    

    public GameObject owner;

    [SerializeField] GameObject objToPool;
    [SerializeField] int poolSize;
    GameObject container;
    List<GameObject> active = new List<GameObject>();
    List<GameObject> inactive = new List<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        container = new GameObject(transform.name + "Pool");
        FillPool();
    }

    /// <summary>
    /// Instantiates objects into the pool
    /// </summary>
    void FillPool()
    {
        int objsToSpawn = poolSize - (active.Count + inactive.Count);
        for (int i = 0; i < objsToSpawn; i++)
        {
            GameObject obj = Instantiate(objToPool, container.transform);
            inactive.Add(obj);
            obj.AddComponent<PooledObj>();
            obj.GetComponent<PooledObj>().owner = this;
            obj.SetActive(false);

        }
    }
    /// <summary>
    /// Returns and enables an object from the pool
    /// </summary>
    /// <returns></returns>
    public GameObject SpawnObj()
    {
        GameObject obj;
        if(inactive.Count > 0)
        {
            obj = inactive[0];
            inactive.RemoveAt(0);
            active.Add(obj);

        }
        else
        {
            poolSize *= 2;
            FillPool();
            obj = inactive[0];
            inactive.RemoveAt(0);
            active.Add(obj);
        }
        obj.SetActive(true);
        return obj;
    }

    public void DespawnAllActive()
	{
        GameObject[] temp = new GameObject[active.Count];
        active.CopyTo(temp, 0);
		foreach (GameObject obj in temp)
		{
            DespawnObj(obj);
		}
	}
    /// <summary>
    /// Disables and objects and returns it too the inactive pool;
    /// </summary>
    /// <param name="obj">the object to despawn</param>
    public void DespawnObj(GameObject obj)
    {
        if (active.Contains(obj))
        {
            active.Remove(obj);
            inactive.Add(obj);
            obj.SetActive(false);
        }
    }
    private void OnDestroy()
    {
        Destroy(container);
    }
}
