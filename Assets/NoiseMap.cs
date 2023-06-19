using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMap : MonoBehaviour
{
    Vector2 noiseOffset;
    [SerializeField] float scale = 1, amplitude;
    public float SampleNoise(Vector3 position)
    {
        Vector2 pos = new Vector2(position.x, position.z) + noiseOffset;
        pos *= scale;
        return (gradientNoise(pos) + .5f) * amplitude;
    }
    Vector2 gradientNoisedir(Vector2 p)
    {
        p = new Vector2(p.x % 289, p.y % 289);
        float x = (34 * p.x + 1) * p.x % 289 + p.y;
        x = (34 * x + 1) * x % 289;
        x = ((x / 41) % 1) * 2 - 1;
        return (new Vector2(x - Mathf.Floor(x + 0.5f), Mathf.Abs(x) - 0.5f)).normalized;
    }

    float gradientNoise(Vector2 p)
    {
        Vector2 ip = new Vector2(Mathf.Floor(p.x), Mathf.Floor(p.y));
        Vector2 fp = new Vector2(p.x % 1, p.y % 1);
        float d00 = Vector3.Dot(gradientNoisedir(ip), fp);
        float d01 = Vector3.Dot(gradientNoisedir(ip + new Vector2(0, 1)), fp - new Vector2(0, 1));
        float d10 = Vector3.Dot(gradientNoisedir(ip + new Vector2(1, 0)), fp - new Vector2(1, 0));
        float d11 = Vector3.Dot(gradientNoisedir(ip + new Vector2(1, 1)), fp - new Vector2(1, 1));
        fp = fp * fp * fp * (fp * (fp * 6 - new Vector2(15, 15)) + new Vector2(10, 10));
        return Mathf.Lerp(Mathf.Lerp(d00, d01, fp.y), Mathf.Lerp(d10, d11, fp.y), fp.x);
    }




}
