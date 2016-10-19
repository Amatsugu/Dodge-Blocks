using ProtoBuf;

namespace LuminousVector.LevelGenerator
{
	[ProtoContract]
	public class Voxel
	{
		public string poolID { get { return _poolID; } }

		[ProtoMember(1)]
		private string _poolID;

		Voxel()
		{

		}

		public Voxel(string poolID)
		{
			_poolID = poolID;
		}
	}
}
