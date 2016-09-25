using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LuminousVector
{
	public class ObjectPoolerWorld : MonoBehaviour
	{
		public GameObject pooledObject;
		public int poolSize = 20;
		public bool willGrow = false;

		public List<GameObject> _objectPool;

		void Start()
		{
			_objectPool = new List<GameObject>();
			for (int i = 0; i < poolSize; i++)
			{
				GameObject obj = Instantiate(pooledObject) as GameObject;
				obj.SetActive(false);
				obj.transform.parent = transform;
				_objectPool.Add(obj);
			}
		}

		public GameObject GetPooledObject()
		{
			for (int i = 0; i < _objectPool.Count; i++)
			{
				if (!_objectPool[i].activeInHierarchy)
				{
					return _objectPool[i];
				}
			}
			if (willGrow)
			{
				GameObject obj = Instantiate(pooledObject) as GameObject;
				_objectPool.Add(obj);
				obj.transform.parent = transform;
				return obj;
			}

			return null;
		}

		public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			GameObject g = GetPooledObject();
			if (g == null)
				return g;
			g.transform.position = position;
			g.transform.rotation = rotation;
			g.transform.parent = parent;
			g.SetActive(true);
			return g;
		}
	}
}
