using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectPooler 
{
	public GameObject pooledObject;
	public int poolSize = 20;
	public bool willGrow = false;

	public List<GameObject> _objectPool;

	public void CreateObjectPool(GameObject Obj, int size, bool grow) 
	{
		poolSize = size;
		pooledObject = Obj;
		willGrow = grow;
		_objectPool = new List<GameObject>();
		for(int i = 0; i < poolSize; i++)
		{
			GameObject obj = GameObject.Instantiate(pooledObject) as GameObject;
			obj.SetActive(false);
			_objectPool.Add(obj);
		}
	}

	public GameObject GetPooledObject()
	{
		for(int i = 0; i < _objectPool.Count; i++)
		{
			if(!_objectPool[i].activeInHierarchy)
			{
				return _objectPool[i];
			}
		}
		if(willGrow)
		{
			GameObject obj = GameObject.Instantiate(pooledObject) as GameObject;
			_objectPool.Add(obj);
			return obj;
		}

		return null;
	}
}
