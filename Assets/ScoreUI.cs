using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI wavesCount;
    [SerializeField] TextMeshProUGUI killsCount;

    public void UpdateBoard(int kills, int wave)
    {
        wavesCount.text = wave.ToString();
        killsCount.text = kills.ToString();
    }
}
