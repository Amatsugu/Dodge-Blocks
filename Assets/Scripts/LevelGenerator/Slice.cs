using ProtoBuf;
using UnityEngine;
using System.Collections.Generic;
using LuminousVector.LevelGenerator.Execeptions;

namespace LuminousVector.LevelGenerator
{
	[ProtoContract]
	public class Slice
	{
		public Vector2 size { get { return new Vector2(_width, _height); } }
		public int curVoxel { get { return _curVoxel; } }

		[ProtoMember(1)]
		private int _width;
		[ProtoMember(2)]
		private int _height;
		[ProtoMember(3)]
		private List<Voxel> _voxels;
		private int _curVoxel = 0;

		Slice()
		{

		}

		public Voxel NextVoxel()
		{
			return _voxels[_curVoxel++];
		}

		public Voxel GetVoxel(int x, int y)
		{
			return _voxels[_width * y + x];
		}

		public Slice(int width, int height, List<Voxel> voxels)
		{
			if (voxels.Count != width * height)
				throw new InvalidSliceExeception("Size of voxels provided does not match size specified " + (width*height) + " != " + voxels.Count);
			_height = height;
			_width = width;
			(_voxels = new List<Voxel>()).AddRange(voxels);
		}
	}
}
