using UnityEngine;
using System.Linq;
using System.Collections.Generic;

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


		void Update()
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
					_ray.origin = new Vector3()
					{
						x = x + _motor.basePos.x,
						y = y + _motor.basePos.y,
						z = _motor.curPos.z
					};
					if (!Physics.Raycast(_ray, out _hit, range))
					{
						Debug.DrawLine(_ray.origin, _ray.GetPoint(range), Color.magenta);
						_nodes.Add(new AINode()
						{
							distance = float.PositiveInfinity,
							position = new Vector3(x,y)
						});
					}else
					{
						Debug.DrawLine(_ray.origin, _ray.GetPoint(range), Color.cyan);
						_nodes.Add(new AINode()
						{
							distance = _hit.distance,
							position = new Vector3(x,y)
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
					_ray.origin = new Vector3()
					{
						x = x + _motor.basePos.x,
						y = y + _motor.basePos.y,
						z = _motor.curPos.z - rearSight
					};
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
