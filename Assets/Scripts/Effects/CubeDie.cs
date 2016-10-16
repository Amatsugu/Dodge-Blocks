using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	public class CubeDie : MonoBehaviour
	{

		public GameObject deathShatter;
		public ObjectPoolerWorld pool;

		void Start()
		{
			pool = ObjectPoolManager.GetPool("ShatterPool");
		}

		public void Die()
		{
			pool.Instantiate(transform.position, Quaternion.identity);
		}
	}
}
