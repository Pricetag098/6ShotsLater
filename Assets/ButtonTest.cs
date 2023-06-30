using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ButtonTest : MonoBehaviour
{
    Rigidbody rb;
    SoundPlayer player;
    public float pressPoint;
    public UnityEvent OnPress;
    bool pressed;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.sleepThreshold = -1;
        player = GetComponent<SoundPlayer>();   
    }

    // Update is called once per frame
    void Update()
    {
        if (!pressed)
        {
            if (transform.localPosition.y < pressPoint)
            {
                OnPress.Invoke();
                player.Play();
                pressed = true;
            }
        }
		else
		{
            if (transform.localPosition.y > pressPoint)
			{
                pressed = false;
			}
		}
        
    }
}
