using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuminousVector
{
	public class PooledGameObject
	{
		public GameObject gameObject { get; set; }
		public bool activeInHierarchy
		{
			get
			{
				return gameObject.activeInHierarchy;
			}
		}
		public Transform transform
		{
			get
			{
				return gameObject.transform;
			}
		}
		public Rigidbody rigidBody
		{
			get
			{
				return _thisRigidBody;
			}
		}

		private Rigidbody _thisRigidBody;

		private ObjectPoolerWorld _objectPool;

		public PooledGameObject(ObjectPoolerWorld objectPool, GameObject gameObject)
		{
			_objectPool = objectPool;
			this.gameObject = gameObject;
			_thisRigidBody = gameObject.GetComponent<Rigidbody>();
		}

		public ObjectPoolerWorld Destroy()
		{
			return _objectPool.Destroy(gameObject);
		}

		public void SetActive(bool active)
		{
			gameObject.SetActive(active);
		}
	}
}
