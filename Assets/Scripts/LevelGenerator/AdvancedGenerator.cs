using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector.LevelGenerator
{
	public class AdvancedGenerator : MonoBehaviour 
	{

		public float planeJittter = .1f;
		public bool rebuild = true;

		private ObjectPoolerWorld _floorVoxelPool;
		private List<PooledGameObject> _floorVoxels;
		private List<PooledGameObject> _worldVoxels;
		private Vector3 _curPos;
		private Vector3 _curDir = new Vector3(0, 0, 1);
		private Vector3 _curUp = new Vector3(0, 1, 0);
		private bool _inverted
		{
			get
			{
				if (_curUp == Vector3.up && (_curDir == Vector3.left || _curDir == Vector3.right)) //UP Left/Right 
					return false;
				else if (_curUp == Vector3.down && (_curDir == Vector3.left || _curDir == Vector3.right)) //DOWN Left/Right
					return true;
				else if (_curUp == Vector3.up && (_curDir == Vector3.forward || _curDir == Vector3.back)) //UP forward/back
					return false;
				else if (_curUp == Vector3.down && (_curDir == Vector3.forward || _curDir == Vector3.back)) //DOWN forward/back
					return true;
				else if (_curUp == Vector3.left && (_curDir == Vector3.forward || _curDir == Vector3.back)) //LEFT forward/Back
					return true;
				else if (_curUp == Vector3.right && (_curDir == Vector3.forward || _curDir == Vector3.back)) //RIGHT forward/Back
					return false;
				else if (_curUp == Vector3.left && (_curDir == Vector3.up || _curDir == Vector3.down)) //LEFT up/down
					return true;
				else if (_curUp == Vector3.right && (_curDir == Vector3.up || _curDir == Vector3.down)) //RIGHT up/down
					return false;
				else if (_curUp == Vector3.forward && (_curDir == Vector3.up || _curDir == Vector3.down)) //FORWARD up/down
					return false;
				else if (_curUp == Vector3.back && (_curDir == Vector3.up || _curDir == Vector3.down)) //BACK up/down
					return true;
				else if (_curUp == Vector3.forward && (_curDir == Vector3.right || _curDir == Vector3.left)) //FORWARD right/left
					return false;
				else if (_curUp == Vector3.back && (_curDir == Vector3.right || _curDir == Vector3.left)) //BACK right/left
					return true;

				return false; //Default
			}
		}
		private PlaneAxis _curAxis
		{
			get
			{
				if ((_curUp == Vector3.down || _curUp == Vector3.up) && (_curDir == Vector3.left || _curDir == Vector3.right))
					return PlaneAxis.ZX;
				else if ((_curUp == Vector3.back || _curUp == Vector3.forward) && (_curDir == Vector3.up || _curDir == Vector3.down))
					return PlaneAxis.XY;
				else if ((_curUp == Vector3.left || _curUp == Vector3.right) && (_curDir == Vector3.up || _curDir == Vector3.down))
					return PlaneAxis.ZY;
				else if ((_curUp == Vector3.back || _curUp == Vector3.forward) && (_curDir == Vector3.left || _curDir == Vector3.right))
					return PlaneAxis.YX;
				else if ((_curUp == Vector3.left || _curUp == Vector3.right) && (_curDir == Vector3.forward || _curDir == Vector3.back))
					return PlaneAxis.YZ;
				else
					return PlaneAxis.XZ;

			}
		}
		private Level _curLevel;

		void Start()
		{
			_floorVoxelPool = ObjectPoolManager.GetPool("FloorVoxelPool");
			_floorVoxels = new List<PooledGameObject>();
			_curPos = Vector3.zero;
			_worldVoxels = new List<PooledGameObject>();
			_curLevel = new RandomGenLevel("Random Level")
			{
				length = new Uitls.ValueRange(10, 30),
				width = 5,
				height = 3,
				poolID = "VoxelPool",
				density = 10,
				turnFequency = new Uitls.ValueRange(1, 4)
			}.GenerateZone();
			AlignPos();
		}

		void AlignPos()
		{
			switch (_curAxis)
			{
				case PlaneAxis.XZ:
					_curPos.x -= (int)_curLevel.currentZone.size.x / 2;
					break;
				case PlaneAxis.XY:
					_curPos.x -= (int)_curLevel.currentZone.size.x / 2;
					break;
				case PlaneAxis.ZX:
					_curPos.z -= (int)_curLevel.currentZone.size.x / 2;
					break;
				case PlaneAxis.ZY:
					_curPos.z -= (int)_curLevel.currentZone.size.x / 2;
					break;
				case PlaneAxis.YX:
					_curPos.y -= (int)_curLevel.currentZone.size.x / 2;
					break;
				case PlaneAxis.YZ:
					_curPos.y -= (int)_curLevel.currentZone.size.x / 2;
					break;
				default:
					return;
			}
		}

		void Update()
		{
			if (!rebuild)
				return;
			foreach (PooledGameObject g in _floorVoxels)
				g.Destroy();
			foreach (PooledGameObject g in _worldVoxels)
				g.Destroy();
			Start();
			for(int i = 0; i < 50; i++)
			{
				BuildZone(_curLevel.NextZone());
			}
			rebuild = false;
		}

		//Zone Builder
		void BuildZone(Zone zone)
		{
			if (zone.GetType() == typeof(TurnZone))
			{
				BuildTurnZone((TurnZone)zone);
				return;
			}
			for (int i = 0; i < zone.size.z; i++)
			{
				BuildZoneSlice(zone.NextSlice(), _curPos, _curAxis, _inverted);
				BuildFloorSlice((int)zone.size.x, _curPos, _curAxis, _inverted);
				_curPos += _curDir;
			}
		}


		//Zone Slice Builder
		void BuildZoneSlice(Slice slice, Vector3 basePos, PlaneAxis axis, bool inverted = false)
		{
			Vector3 pos;
			Voxel curVoxel;
			for(int y = 0; y < slice.size.y; y++)
			{
				for(int x = 0; x < slice.size.x; x++)
				{
					if ((curVoxel = slice.Next()) == null)
						continue;
					int x1 = x + (int)slice.size.x/2, 
						y1 = (inverted) ? (int)_curLevel.currentZone.size.y - y : y;
					switch (axis)
					{
						case PlaneAxis.XZ:
							pos = new Vector3(x1, y1, 0);
							break;
						case PlaneAxis.XY:
							pos = new Vector3(x1, 0, y1);
							break;
						case PlaneAxis.ZX:
							pos = new Vector3(0, y1, x1);
							break;
						case PlaneAxis.ZY:
							pos = new Vector3(y1, 0, x1);
							break;
						case PlaneAxis.YX:
							pos = new Vector3(0, x1, y1);
							break;
						case PlaneAxis.YZ:
							pos = new Vector3(y1, x1, 0);
							break;
						default:
							return;
					}
					pos += basePos;
					_worldVoxels.Add(ObjectPoolManager.GetPool(curVoxel.poolID).Instantiate(pos, Quaternion.identity, transform));
				}
			}
		}


		//Turn Zone Builder
		void BuildTurnZone(TurnZone zone)
		{

			RotationAxis axis;
			switch(zone.turnDir)
			{
				case TurnDir.Up:
					if (_curAxis == PlaneAxis.XZ || _curAxis == PlaneAxis.XY)
						axis = RotationAxis.X;
					else if (_curAxis == PlaneAxis.YX || _curAxis == PlaneAxis.YZ)
						axis = RotationAxis.Y;
					else
						axis = RotationAxis.Z;
					_curDir = Utils.Rotate(_curDir, Mathf.PI / -2f, axis);
					_curUp = Utils.Rotate(_curUp, Mathf.PI / -2f, axis);

					break;
				case TurnDir.Left:
					if (_curAxis == PlaneAxis.XZ || _curAxis == PlaneAxis.ZX)
						axis = RotationAxis.Y;
					else if (_curAxis == PlaneAxis.XY || _curAxis == PlaneAxis.YX)
						axis = RotationAxis.Z;
					else //if (_curAxis == PlaneAxis.YZ || _curAxis == PlaneAxis.ZY)
						axis = RotationAxis.X;
					_curDir = Utils.Rotate(_curDir, Mathf.PI / -2f, axis);
					_curUp = Utils.Rotate(_curUp, Mathf.PI / -2f, axis);
					break;
				case TurnDir.Right:
					if (_curAxis == PlaneAxis.XZ || _curAxis == PlaneAxis.ZX)
						axis = RotationAxis.Y;
					else if (_curAxis == PlaneAxis.XY || _curAxis == PlaneAxis.YX)
						axis = RotationAxis.Z;
					else //if (_curAxis == PlaneAxis.YZ || _curAxis == PlaneAxis.ZY)
						axis = RotationAxis.X;
					_curDir = Utils.Rotate(_curDir, Mathf.PI / 2f, axis);
					_curUp = Utils.Rotate(_curUp, Mathf.PI / 2f, axis);
					break;
				case TurnDir.Down:
					if (_curAxis == PlaneAxis.XZ || _curAxis == PlaneAxis.XY)
						axis = RotationAxis.X;
					else if (_curAxis == PlaneAxis.YX || _curAxis == PlaneAxis.YZ)
						axis = RotationAxis.Y;
					else
						axis = RotationAxis.Z;
					_curDir = Utils.Rotate(_curDir, Mathf.PI / 2f, axis);
					_curUp = Utils.Rotate(_curUp, Mathf.PI / 2f, axis);
					break;
			}
			_curPos += _curDir * zone.size.z;
			//AlignPos();
		}

		//Floor Builder
		void BuildFloorSlice(int width, Vector3 basePos, PlaneAxis axis, bool inverted = false)
		{
			Vector3 pos;
			for (float x = 0; x < width; x++)
			{
				float jitterValue = Random.Range(-planeJittter, planeJittter) - 1;
				jitterValue = ((inverted) ? _curLevel.currentZone.size.y - jitterValue : jitterValue);
				switch (axis)
				{
					case PlaneAxis.XZ:
						pos = new Vector3(x, jitterValue, 0);
						break;
					case PlaneAxis.XY:
						pos = new Vector3(x, 0, jitterValue);
						break;
					case PlaneAxis.ZX:
						pos = new Vector3(0, jitterValue, x);
						break;
					case PlaneAxis.ZY:
						pos = new Vector3(jitterValue, 0, x);
						break;
					case PlaneAxis.YX:
						pos = new Vector3(0, x, jitterValue);
						break;
					case PlaneAxis.YZ:
						pos = new Vector3(jitterValue, x, 0);
						break;
					default:
						return;
				}
				pos += basePos;
				_floorVoxels.Add(_floorVoxelPool.Instantiate(pos, Quaternion.identity, transform));
			}
		}
	}
}
