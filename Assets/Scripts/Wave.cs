using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave")]
public class Wave : ScriptableObject
{
    public static int zombieTypeCount = 8;
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
            h
        }
        [SerializeField]
        public ZombieTypes zombieType;
        [SerializeField]
        public int num;
    }

    public List<SpawnRequest> spawnList = new List<SpawnRequest>();
}
