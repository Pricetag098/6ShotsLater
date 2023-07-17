using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBox : MonoBehaviour
{
    ObjectPooler objectPooler;
    [SerializeField] int stuffInBox;

	private void OnTriggerExit(Collider other)
	{
        GameObject go = objectPooler.Spawn();
        go.transform.position = transform.position;
    }
	// Start is called before the first frame update
	void Start()
    {
        objectPooler = GetComponent<ObjectPooler>();
        for(int i = 0; i < stuffInBox; i++)
		{
            GameObject go = objectPooler.Spawn();
            go.transform.position = transform.position;

		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
