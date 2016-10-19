using UnityEngine;
using ProtoBuf;
using System.Collections.Generic;

namespace LuminousVector.LevelGenerator
{
	[ProtoContract]
	public class Level 
	{
		public string name { get { return _name; } }
		public Zone currentZone { get { return _zones[_curZone]; } }
		public bool isEnd { get { return _isEnd; } }

		[ProtoMember(1)]
		protected List<Zone> _zones;
		[ProtoMember(2)]
		protected string _name;
		protected int _curZone = 0;
		protected bool _isEnd = false;

		public Level(string name, IEnumerable<Zone> zones = null)
		{
			_name = name;
			if (zones == null)
				_zones = new List<Zone>();
			else
				(_zones = new List<Zone>()).AddRange(zones);
		}

		public virtual Level AddZone(Zone zone)
		{
			_zones.Add(zone);
			return this;
		}

		public virtual Zone NextZone()
		{
			if (_curZone >= _zones.Count)
				return null;
			else
				return _zones[_curZone++];
		}
	}
}
