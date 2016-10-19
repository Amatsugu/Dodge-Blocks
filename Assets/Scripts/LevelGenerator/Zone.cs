using UnityEngine;
using ProtoBuf;
using System.Collections.Generic;
using LuminousVector.LevelGenerator.Execeptions;

namespace LuminousVector.LevelGenerator
{
	[ProtoContract]
	public class Zone 
	{
		public Vector3 size { get { return new Vector3(_width, _height, _slices.Count); } }
		public Color color { get { return _Color.color; } }

		[ProtoMember(1)]
		protected List<Slice> _slices;
		[ProtoMember(2)]
		protected int _width;
		[ProtoMember(3)]
		protected int _height;
		[ProtoMember(4)]
		protected SColor _Color;
		protected int _curSlice = 0;

		Zone()
		{

		}

		public Zone(int width,int height, IEnumerable<Slice> slices = null)
		{
			_width = width;
			_height = height;
			if (slices == null)
				_slices = new List<Slice>();
			else
				(_slices = new List<Slice>()).AddRange(slices);
		}

		public virtual Zone AddSlice(Slice slice)
		{
			_slices.Add(slice);
			return this;
		}
		
		public virtual Zone AddSlices(IEnumerable<Slice> slices)
		{
			_slices.AddRange(slices);
			return this;
		}

		public virtual Slice NextSlice()
		{
			if (_curSlice >= _slices.Count)
				return null;
			return _slices[_curSlice++];
		}
	}
}
