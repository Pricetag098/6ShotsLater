using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave")]
public class Wave : ScriptableObject
{
    public static int zombieTypeCount = 10;
    [System.Serializable]
    public struct SpawnRequest
    {
        
        public enum ZombieTypes
        {
            a,
            b,
            c,
            d,
            e,
            f,
            g,
            h,
            i,
            j
        }
        [SerializeField]
        public ZombieTypes zombieType;
        [SerializeField]
        public int number;

        public float spawnDelay;
        public float moveSpeedMin;
        public float moveSpeedMax;
    }

    public List<SpawnRequest> spawnList = new List<SpawnRequest>();
}
