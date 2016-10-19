using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector.LevelGenerator
{
	public class LevelObject
	{
		public ObjectPoolerWorld objectPool { get; set; }

		public PooledGameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent = null)
		{
			return objectPool.Instantiate(position, rotation, parent);
		}
	}

}
