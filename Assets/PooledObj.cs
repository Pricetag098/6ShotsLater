using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to be placed on objects in object pools
/// </summary>
public class PooledObj : MonoBehaviour
{
    public ObjectPooler owner;
    public void Despawn()
    {
        owner.DespawnObj(gameObject);
    }
}
