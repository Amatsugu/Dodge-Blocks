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
		[HideInInspector]
		public int seed;

		private ObjectPoolerWorld _floorVoxelPool;
		private List<PooledGameObject> _floorVoxels;
		private List<PooledGameObject> _worldVoxels;
		private List<GameObject> _worldZones;
		private Vector3 _curPos;
		private Vector3 _curDir;
		private Vector3 _curUp;
		[HideInInspector]
		public List<GenPoints> _genPoints = new List<GenPoints>();

		private DateTime _startTime;

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
			seed = DateTime.Now.ToLongTimeString().GetHashCode();
			//seed = -503363039;
			_floorVoxelPool = ObjectPoolManager.GetPool("FloorVoxelPool");
			_floorVoxels = new List<PooledGameObject>();
			_curPos = Vector3.zero;
			_worldVoxels = new List<PooledGameObject>();
			_worldZones = new List<GameObject>();
			_curLevel = new RandomGenLevel("Random Level", seed)
			{
				length = new Uitls.ValueRange(10, 20),
				width = 5,
				height = 3,
				poolID = "VoxelPool",
				density = 10,
				turnFequency = new Uitls.ValueRange(1, 4)
			}.Init().GenerateZone();
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
			_genPoints.Add(new GenPoints(_curPos, _curDir, _curUp));
		}


		//Zone Slice Builder
		void BuildZoneSlice(Slice slice,float curPos, PlaneAxis axis)
		{
			Vector3 pos;
			Voxel curVoxel;
			for (int y = 0; y < slice.size.y; y++)
			{
				for (int x = 0; x < slice.size.x; x++)
				{
					if ((curVoxel = slice.GetVoxel(x,y)) == null)
						continue;
					
					pos = new Vector3(x + (int)(slice.size.x / -2), slice.size.y - y -1, curPos);
					_worldVoxels.Add(ObjectPoolManager.GetPool(curVoxel.poolID).Instantiate(pos, Quaternion.identity, _worldZones[_worldZones.Count-1].transform));
				}
			}
		}

		//Turn Zone Builder
		void BuildTurnZone(TurnZone zone)
		{
			GameObject turn = new GameObject();
			turn.name = "Zone " + _worldZones.Count + " ("+ zone.turnDir +")";
			turn.transform.position = _curPos;
			turn.transform.parent = transform;
			RotationAxis axis;
			float angle = Mathf.PI / 2;
			switch (zone.turnDir)
			{
				case TurnDir.Up: //UP
					for(int i = 1; i <= zone.size.z; i ++)
					{
						BuildFloorSlice((int)zone.size.x, i, _curAxis);
					}
					_curPos += _curDir * (zone.size.z);
					_genPoints.Add(new GenPoints(_curPos, _curDir, _curUp, Color.gray));

					angle *= TurnHelper.TurnUp(_curAxis, _curDir, _curUp, out axis);
					_curDir = Utils.Rotate(_curDir, -angle, axis);
					_curUp = Utils.Rotate(_curUp, -angle, axis);
					_curPos += _curDir * (zone.size.y);
					break;
				case TurnDir.Down: //DOWN
					_curPos += _curDir * (zone.size.z - 1);
					_genPoints.Add(new GenPoints(_curPos, _curDir, _curUp, Color.yellow));

					angle *= TurnHelper.TurnDown(_curAxis, _curDir, _curUp, out axis);
					_curDir = Utils.Rotate(_curDir, angle, axis);
					_curUp = Utils.Rotate(_curUp, angle, axis);
					_curPos += _curDir * (zone.size.y - 1);
					break;
				case TurnDir.Left: //LEFT
					_curPos += _curDir * (zone.size.z);
					_genPoints.Add(new GenPoints(_curPos, _curDir, _curUp, Color.black));

					angle *= TurnHelper.TurnLeft(_curAxis, _curDir, out axis);
					_curDir = Utils.Rotate(_curDir, -angle, axis);
					_curUp = Utils.Rotate(_curUp, -angle, axis);
					_curPos += _curDir * (zone.size.x);
					break;
				case TurnDir.Right: //RIGHT
					_curPos += _curDir * (zone.size.z);
					_genPoints.Add(new GenPoints(_curPos, _curDir, _curUp, Color.blue));

					angle *= TurnHelper.TurnRight(_curAxis, _curUp, out axis);
					_curDir = Utils.Rotate(_curDir, angle, axis);
					_curUp = Utils.Rotate(_curUp, angle, axis);
					_curPos += _curDir * (zone.size.x);
					break;

			}
			_genPoints.Add(new GenPoints(_curPos, _curDir, _curUp));
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
