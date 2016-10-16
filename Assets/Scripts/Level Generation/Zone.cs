using UnityEngine;
using System.Collections.Generic;
using LuminousVector.LevelGenerator.Execeptions;

namespace LuminousVector.LevelGenerator
{
	public class Zone 
	{
		public Vector2 size
		{
			get
			{
				_size.z = _slices.Count;
				return _size;
			}
		}

		private List<Slice> _slices;
		private Vector3 _size;
		private int _curSlice = 0;

		public Zone(int height, int width, IEnumerable<Slice> slices = null)
		{
			_size = new Vector3(height, width);
			if (slices == null)
				_slices = new List<Slice>();
			else
				(_slices = new List<Slice>()).AddRange(slices);
		}

		public Zone AddSlice(Slice slice)
		{
			if (_size.x == slice.size.x && _size.y == slice.size.y)
				throw new SliceZoneMismatchExeception(slice.size, _size);
			_slices.Add(slice);
			return this;
		}

		public Slice NextSlice()
		{
			if (_curSlice >= _slices.Count)
				return null;
			return _slices[_curSlice++];
		}
	}
}
