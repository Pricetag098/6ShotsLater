using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
public class PlayerHealth : MonoBehaviour
{

    Health health;
    [SerializeField]VolumeProfile profile;
    [SerializeField] float maxVinette = 1;
    public Wavemanager wavemanager;

    [SerializeField] float healthPerSecond;
    [SerializeField] CanvasGroup canvasGroup;
    bool dead = false;
    AsyncOperation op;
    // Start is called before the first frame update
    void Start()
    {
        wavemanager = FindObjectOfType<Wavemanager>();
        health = GetComponent<Health>();
        health.OnHit += OnHit;
        health.OnDeath += Die;
    }

    private void Update()
    {
        health.TakeDmg(-healthPerSecond * Time.deltaTime);
        if (dead)
        {
            canvasGroup.alpha += Time.deltaTime;
            if(canvasGroup.alpha >= 1)
            {
                op.allowSceneActivation = true;
            }
        }
        else
        {
            canvasGroup.alpha -= Time.deltaTime;
        }
    }

    void OnHit()
	{
        UnityEngine.Rendering.Universal.Vignette vignette;
        if(profile.TryGet(out vignette))
		{
            float vinetteVal = (1 - (health.health / health.maxHealth)) * maxVinette;
            vignette.intensity.value = vinetteVal * maxVinette;
		}
    }
    void Die()
	{
        if (!dead)
        {
            UnityEngine.Rendering.Universal.Vignette vignette;
            if (profile.TryGet(out vignette))
            {
                //float vinetteVal = 1 - (health.health / health.maxHealth);
                vignette.intensity.value = 0;
            }
            op =  SceneManager.LoadSceneAsync(0);
            op.allowSceneActivation = false;
            wavemanager.state = Wavemanager.States.dead;
            dead = true;

        }
        
	}
    private void OnDestroy()
    {
        UnityEngine.Rendering.Universal.Vignette vignette;
        if (profile.TryGet(out vignette))
        {
            //float vinetteVal = 1 - (health.health / health.maxHealth);
            vignette.intensity.value = 0;
        }
    }

}
