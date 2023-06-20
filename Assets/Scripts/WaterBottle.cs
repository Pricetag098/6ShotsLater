using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBottle : MonoBehaviour
{
    ParticleSystem particle;
    public float pourCutoff = 0;
    bool playing = false;
    // Start is called before the first frame update
    void Start()
    {
        particle = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        float direction = Vector3.Dot(transform.up,Vector3.up);
		if (playing && direction > pourCutoff)
		{
            particle.Stop();
            playing = false;
		}
		if(!playing && direction < pourCutoff)
		{
            particle.Play();
            playing = true;
        }
    }
}
