using UnityEngine;
using System;
using System.Collections.Generic;

namespace LuminousVector.LevelGenerator
{
	public class AdvancedGenerator : MonoBehaviour 
	{

		public float planeJittter = .1f;
		public int zoneCount = 100;
		public bool rebuild = true;

		private ObjectPoolerWorld _floorVoxelPool;
		private List<PooledGameObject> _floorVoxels;
		private List<PooledGameObject> _worldVoxels;
		private List<GameObject> _worldZones;
		private Vector3 _curPos;
		private Vector3 _curDir;
		private Vector3 _curUp;
		private List<GenPoints> _genPoints = new List<GenPoints>();

		private DateTime _startTime;
		private int t = 0;

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
			_startTime = DateTime.Now;
			_curDir = Vector3.forward;
			_curUp = Vector3.up;
			_floorVoxelPool = ObjectPoolManager.GetPool("FloorVoxelPool");
			_floorVoxels = new List<PooledGameObject>();
			_curPos = Vector3.zero;
			_worldVoxels = new List<PooledGameObject>();
			_worldZones = new List<GameObject>();
			_curLevel = new RandomGenLevel("Random Level")
			{
				length = new Uitls.ValueRange(10, 30),
				width = 5,
				height = 3,
				poolID = "VoxelPool",
				density = 10,
				turnFequency = new Uitls.ValueRange(1, 4)
			}.GenerateZone();
			//AlignPos();
		}


		void Update()
		{
			for(int i = 1; i < _genPoints.Count; i++)
			{
				GenPoints pos = _genPoints[i];
				Debug.DrawLine(pos.pos, pos.pos + pos.up * 3, pos.col);
				Debug.DrawLine(_genPoints[i - 1].pos, pos.pos, Color.cyan);
			}
			if (!rebuild)
				return;
			_genPoints.Clear();
			foreach (PooledGameObject g in _floorVoxels)
				g.Destroy();
			foreach (PooledGameObject g in _worldVoxels)
				g.Destroy();
			foreach (GameObject g in _worldZones)
				Destroy(g);
			Start();
			for(int i = 0; i < zoneCount; i++)
			{
				BuildZone(_curLevel.NextZone());
			}
			rebuild = false;
			Debug.Log("Generated " + zoneCount + " in " + (DateTime.Now -_startTime).TotalMilliseconds + "ms");
		}

		//Zone Builder
		void BuildZone(Zone zone)
		{
			if (zone.GetType() == typeof(TurnZone))
			{
				BuildTurnZone((TurnZone)zone);
				return;
			}
			GameObject curZone = new GameObject();
			curZone.transform.position = _curPos;
			curZone.transform.rotation = Quaternion.LookRotation(_curDir, _curUp);
			curZone.name = "Zone " + _worldZones.Count;
			curZone.transform.parent = transform;
			_worldZones.Add(curZone);
			for (int i = 0; i < zone.size.z; i++)
			{

				BuildZoneSlice(zone.NextSlice(), i, _curAxis);
				BuildFloorSlice((int)zone.size.x, i, _curAxis);
			}
			_curPos += _curDir * (zone.size.z - 1);
			_genPoints.Add(new GenPoints(_curPos, _curUp));
		}


		//Zone Slice Builder
		void BuildZoneSlice(Slice slice,float curPos, PlaneAxis axis)
		{
			Vector3 pos;
			Voxel curVoxel;
			float hwidth = _curLevel.currentZone.size.x/2;
			for (int y = 0; y < slice.size.y; y++)
			{
				for (int x = 0; x < slice.size.x; x++)
				{
					if ((curVoxel = slice.Next()) == null)
						continue;

					float x1 = x + (int)(slice.size.x / -2);
					pos = new Vector3(x1, y, curPos);
					_worldVoxels.Add(ObjectPoolManager.GetPool(curVoxel.poolID).Instantiate(pos, Quaternion.identity, _worldZones[_worldZones.Count-1].transform));
				}
			}
		}

		//Turn Zone Builder
		void BuildTurnZone(TurnZone zone)
		{
			Debug.Log(zone.turnDir + " (" + (++t) + ")");
			GameObject turn = new GameObject();
			turn.name = "turn " + t;
			turn.transform.position = _curPos;
			turn.transform.parent = transform;
			RotationAxis axis;
			float angle = Mathf.PI / 2;
			switch(zone.turnDir)
			{
				case TurnDir.Up:
					_curPos += _curDir* (zone.size.z);
					_genPoints.Add(new GenPoints(_curPos, _curUp, Color.gray));
					if (_curAxis == PlaneAxis.XZ || _curAxis == PlaneAxis.XY)
					{
						axis = RotationAxis.X;
						if (_curDir == Vector3.forward && _curUp == Vector3.down)
							angle *= -1;
						if (_curDir == Vector3.up && _curUp == Vector3.forward)
							angle *= -1;
						if (_curDir == Vector3.down && _curUp == Vector3.back)
							angle *= -1;
						if (_curDir == Vector3.back && _curUp == Vector3.up)
							angle *= -1;

					}
					else if (_curAxis == PlaneAxis.YX || _curAxis == PlaneAxis.YZ)
					{
						axis = RotationAxis.Y;
						if (_curDir == Vector3.forward && _curUp == Vector3.right)
							angle *= -1;
						if (_curDir == Vector3.left && _curUp == Vector3.forward)
							angle *= -1;
					}
					else
					{
						axis = RotationAxis.Z;
						if (_curDir == Vector3.down && _curUp == Vector3.right)
							angle *= -1;
					}
					_curDir = Utils.Rotate(_curDir, -angle, axis);
					_curUp = Utils.Rotate(_curUp, -angle, axis);
					_curPos += _curDir * (zone.size.y);
					break;
				case TurnDir.Left:
					_curPos += _curDir * (zone.size.z);
					_genPoints.Add(new GenPoints(_curPos, _curUp, Color.black));
					if (_curAxis == PlaneAxis.XZ || _curAxis == PlaneAxis.ZX)
					{
						axis = RotationAxis.Y;
						if (_curDir == Vector3.up && _curUp == Vector3.back)
							angle *= -1;
					}
					else if (_curAxis == PlaneAxis.XY || _curAxis == PlaneAxis.YX)
					{
						axis = RotationAxis.Z;

					}
					else
					{
						axis = RotationAxis.X;

					}
					_curDir = Utils.Rotate(_curDir, -angle, axis);
					_curUp = Utils.Rotate(_curUp, -angle, axis);
					_curPos += _curDir * (zone.size.x);
					break;
				case TurnDir.Right:
					_curPos += _curDir * (zone.size.z);
					_genPoints.Add(new GenPoints(_curPos, _curUp, Color.blue));
					if (_curAxis == PlaneAxis.XZ || _curAxis == PlaneAxis.ZX)
					{
						axis = RotationAxis.Y;
						if (_curDir == Vector3.down && _curUp == Vector3.up)
							angle *= -1;
					}
					else if (_curAxis == PlaneAxis.XY || _curAxis == PlaneAxis.YX)
					{
						axis = RotationAxis.Z;
						
					}
					else
					{
						axis = RotationAxis.X;
						
					}
					_curDir = Utils.Rotate(_curDir, angle, axis);
					_curUp = Utils.Rotate(_curUp, angle, axis);
					_curPos += _curDir * (zone.size.x);
					break;
				case TurnDir.Down:
					_curPos += _curDir * (zone.size.z-1);
					_genPoints.Add(new GenPoints(_curPos, _curUp, Color.yellow));
					if (_curAxis == PlaneAxis.XZ || _curAxis == PlaneAxis.XY)
					{
						axis = RotationAxis.X;
						
					}
					else if (_curAxis == PlaneAxis.YX || _curAxis == PlaneAxis.YZ)
					{
						axis = RotationAxis.Y;
						if (_curDir == Vector3.left && _curUp == Vector3.forward)
							angle *= -1;
						if (_curDir == Vector3.right && _curUp == Vector3.back)
							angle *= -1;
						if (_curDir == Vector3.back && _curUp == Vector3.left)
							angle *= -1;
						if (_curDir == Vector3.forward && _curUp == Vector3.right)
							angle *= -1;
					}
					else
					{
						axis = RotationAxis.Z;
						if (_curDir == Vector3.right && _curUp == Vector3.up)
							angle *= -1;
					}
					_curDir = Utils.Rotate(_curDir, angle, axis);
					_curUp = Utils.Rotate(_curUp, angle, axis);
					_curPos += _curDir * (zone.size.y -1);
					break;
			}
			_genPoints.Add(new GenPoints(_curPos, _curUp));
		}

		//Floor Builder
		void BuildFloorSlice(int width, float curPos, PlaneAxis axis)
		{
			Vector3 pos;
			for (float x = 0; x < width; x++)
			{
				float jitterValue = UnityEngine.Random.Range(-planeJittter, planeJittter) - 1;
				float x1 = x + (width / -2f);
				x1 += .5f;
				pos = new Vector3(x1, jitterValue, curPos);
				_floorVoxels.Add(_floorVoxelPool.Instantiate(pos, Quaternion.identity, _worldZones[_worldZones.Count - 1].transform));
			}
		}
	}
}
