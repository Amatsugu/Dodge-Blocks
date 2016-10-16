using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System;

namespace LuminousVector
{
	public class AIController : Controller
	{
		public float range = 50;
		[Range(0,1)]
		public float innerRangeMulti = .5f;
		public float rearSight = 1;
		public float distPStrafe = 0.5f;

		private Ray _ray = new Ray(Vector3.zero, Vector3.forward);
		private RaycastHit _hit;
		private List<AINode> _nodes = new List<AINode>();
		private AINode _selectedNode;
		private bool _posFound = false;
		private Vector3 _origin;
		private Vector3 _curPos = new Vector3();


		protected override void Control()
		{
			_nodes.Clear();
			_selectedNode = null;
			//distPStrafe = ((_motor.curVel * Time.deltaTime) * (_motor.strafeSpeed));
			if (_posFound)
			{
				if (Physics.Raycast(_motor.curPos, Vector3.forward, range * innerRangeMulti))
					_posFound = false;
				else
					return;
			}
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					_origin.x = x + _motor.basePos.x;
					_origin.y = y + _motor.basePos.y;
					_origin.z = _motor.curPos.z;
					_ray.origin = _origin;
					_curPos.x = x;
					_curPos.y = y;
					if (!Physics.Raycast(_ray, out _hit, range))
					{
						Debug.DrawLine(_ray.origin, _ray.GetPoint(range), Color.magenta);
						_nodes.Add(new AINode()
						{
							distance = float.PositiveInfinity,
							position = _curPos
						});
					}else
					{
						Debug.DrawLine(_ray.origin, _ray.GetPoint(range), Color.cyan);
						_nodes.Add(new AINode()
						{
							distance = _hit.distance,
							position = _curPos
						});
					}
				}

			}
			foreach(AINode n in from node in _nodes orderby node.GetOffset(strafeVector) descending select node)
			{
				if (_selectedNode == null)
					_selectedNode = n;
				if(n.distance == float.PositiveInfinity)
				{
					if (_selectedNode.GetOffset(strafeVector) > n.GetOffset(strafeVector))
						_selectedNode = n;
				}else
				{
					if (_selectedNode.distance < n.distance)
						_selectedNode = n;
				}
			}
			//Debug.Break();
			if (_selectedNode == null)
				return;
			if (!HasMoveHazard() || Physics.Raycast(_motor.curPos, Vector3.forward, distPStrafe))
			{
				//_posFound = true;
				SetPos(_selectedNode.position);

				if (strafeVector != _lastPos)
					_motor.lerpProgress = 0;

				_lastPos = _motor.strafeVector = strafeVector;
			}
		}

		bool HasMoveHazard()
		{
			bool hazard = false;
			for (int y = -1; y <= 1; y++)
			{
				for (int x = -1; x <= 1; x++)
				{
					if (strafeVector.x == x && strafeVector.y == y)
						continue;
					_origin.x = x + _motor.basePos.x;
					_origin.y = y + _motor.basePos.y;
					_origin.z = _motor.curPos.z - rearSight;
					_ray.origin = _origin;
					Debug.DrawLine(_ray.origin, _ray.GetPoint(distPStrafe + rearSight), Color.red);
					if (Physics.Raycast(_ray, distPStrafe + rearSight))
						hazard = true;
				}
			}
			return hazard;
		}

		void SetPos(Vector3 pos)
		{
			strafeVector.x = Mathf.Clamp(pos.x, -1, 1);
			strafeVector.y = Mathf.Clamp(pos.y, -1, 1);
		}
	}
}
