using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class BuoyantObject : MonoBehaviour
{
    public float buoyantForce;
    [SerializeField] NoiseMap noiseMap;
    [SerializeField] Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(transform.position.y < noiseMap.SampleNoise(transform.position))
        {
            float force = buoyantForce * math.clamp((noiseMap.SampleNoise(transform.position) - transform.position.y), 0, float.PositiveInfinity);
            //Debug.Log(force);
            //rb.AddForce(Vector3.up * force);

        }
        transform.position = new Vector3(transform.position.x, noiseMap.SampleNoise(transform.position),transform.position.z);
    }
}
