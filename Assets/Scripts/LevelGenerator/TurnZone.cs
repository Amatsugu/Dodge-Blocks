using UnityEngine;
using ProtoBuf;

namespace LuminousVector.LevelGenerator
{
	[ProtoContract]
	public class TurnZone : Zone
	{
		public TurnDir turnDir { get { return _turnDir; } }

		public new Vector3 size { get { return new Vector3(_width, _height, _height); } }

		[ProtoMember(1)]
		protected int _length;
		[ProtoMember(2)]
		protected TurnDir _turnDir;

		public TurnZone(int width, int height, int length, TurnDir turnDir) : base(width, height, null)
		{
			_height = height;
			_turnDir = turnDir;
		}

		private new Zone AddSlice(Slice slice)
		{
			return this;
		}

		public override Slice NextSlice()
		{
			return null;
		}
	}
}
