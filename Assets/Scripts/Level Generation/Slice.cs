using System;
using UnityEngine;
using System.Collections.Generic;
using LuminousVector.LevelGenerator.Execeptions;

namespace LuminousVector.LevelGenerator
{
	public class Slice
	{
		public Vector2 size
		{
			get
			{
				return _size;
			}
		}
		public int curVoxel
		{
			get
			{
				return _curVoxel;
			}
		}

		private Vector2 _size;
		private List<GameObject> _voxels;
		private int _curVoxel = 0;

		public GameObject Next()
		{
			return _voxels[_curVoxel++];
		}

		public Slice(int x, int y, List<GameObject> voxels)
		{
			if (voxels.Count != x * y)
				throw new InvalidSliceExeception("Size of voxels provided does not match size specified " + (x*y) + " != " + voxels.Count);
			_size = new Vector2(x, y);
			(_voxels = new List<GameObject>()).AddRange(voxels);
		}
	}
}
