using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace LuminousVector
{
	public class ObjectPoolerWorld : MonoBehaviour
	{
		public GameObject pooledObject;
		public int poolSize = 20;
		public bool willGrow = false;

		private List<PooledGameObject> _objectPool = new List<PooledGameObject>();

		void Start()
		{
			for (int i = 0; i < poolSize; i++)
			{
				GameObject obj = Instantiate(pooledObject) as GameObject;
				obj.SetActive(false);
				obj.transform.parent = transform;
				_objectPool.Add(new PooledGameObject(this, obj));
			}
		}

		public PooledGameObject FindPooledGameObject(GameObject gameObject)
		{
			foreach (PooledGameObject g in _objectPool)
				if(g.gameObject.Equals(gameObject))
					return g;
			return null;
		}

		public PooledGameObject GetPooledObject()
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
				PooledGameObject obj = new PooledGameObject(this, Instantiate(pooledObject) as GameObject);
				_objectPool.Add(obj);
				obj.transform.parent = transform;
				poolSize++;
				return obj;
			}

			return null;
		}

		public PooledGameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			PooledGameObject g = GetPooledObject();
			if (g == null)
				return g;
			g.transform.position = position;
			g.transform.rotation = rotation;
			g.transform.parent = parent;
			g.SetActive(true);
			return g;
		}

		public ObjectPoolerWorld Destroy(GameObject g)
		{
			g.transform.parent = transform;
			g.SetActive(false);
			return this;
		}
	}
}
