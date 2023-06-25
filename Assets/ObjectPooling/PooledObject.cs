using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PooledObject : MonoBehaviour
{
	public ObjectPooler origin;

	/// <summary>
	/// Despawns the object
	/// </summary>
    public void Despawn()
	{
		origin.Despawn(this);
	}	
}
