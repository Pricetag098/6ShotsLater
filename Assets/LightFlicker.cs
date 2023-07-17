using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public float minIntensity = 1f;
    public float maxIntensity = 5f;
    public float minFlickerSpeed = 0.3f;
    public float maxFlickerSpeed = 4f;

    private float flickerSpeed;
    private Light lightSource;

    private void Start()
    {
        lightSource = GetComponent<Light>();
        StartCoroutine(FlickerLight());
    }

    private IEnumerator FlickerLight()
    {
        while (true)
        {
           
            float targetIntensity = Random.Range(minIntensity, maxIntensity);
            while (Mathf.Abs(lightSource.intensity - targetIntensity) > 0.1f)
            {
                lightSource.intensity = Mathf.Lerp(lightSource.intensity, targetIntensity, GetFlickerSpeed() * Time.deltaTime);
                yield return null;
            }
            yield return new WaitForSeconds(Random.Range(0.1f, 0.5f));
        }
    }


    private float GetFlickerSpeed()
    {
       return flickerSpeed = Random.Range(minFlickerSpeed, maxFlickerSpeed);
    }
}
