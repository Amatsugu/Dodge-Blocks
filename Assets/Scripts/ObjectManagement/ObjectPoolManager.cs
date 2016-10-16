using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public class ObjectPoolManager : MonoBehaviour 
	{
		public List<GameObject> objectPoolers;

		public static ObjectPoolManager instance
		{
			get
			{
				if (!OBJECT_POOL_MANAGER)
				{
					OBJECT_POOL_MANAGER = FindObjectOfType<ObjectPoolManager>();
					if (!OBJECT_POOL_MANAGER)
						Debug.LogError("No Object Pool Manager found");
					else
						OBJECT_POOL_MANAGER.Init();
				}
				return OBJECT_POOL_MANAGER;
			}

		}

		private Dictionary<string,ObjectPoolerWorld> _objectPools;
		private static ObjectPoolManager OBJECT_POOL_MANAGER;



		public void Init()
		{
			_objectPools = new Dictionary<string, ObjectPoolerWorld>();
			GameObject curPool;
			foreach(GameObject g in objectPoolers)
			{
				curPool = Instantiate(g, transform) as GameObject;
				_objectPools.Add(g.name, curPool.GetComponent<ObjectPoolerWorld>());
			}
		}

		public static ObjectPoolerWorld GetPool(string poolName)
		{
			if (instance._objectPools.ContainsKey(poolName))
				return instance._objectPools[poolName];
			else
				return null;
		}
	}
}
