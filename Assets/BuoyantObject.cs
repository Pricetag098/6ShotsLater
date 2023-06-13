using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuoyantObject : MonoBehaviour
{
    [SerializeField] float noiseFreq, noiseAmplitude;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Vector2 noiseCoord = new Vector2(pos.x,pos.z) * noiseFreq;
        pos.y = Mathf.PerlinNoise(noiseCoord.x,noiseCoord.y) * noiseAmplitude;
        transform.position = pos;
    }
}
