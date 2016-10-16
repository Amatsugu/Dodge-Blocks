using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector.LevelGenerator
{
	public class Level 
	{
		public string name { get { return _name; } }
		public Zone currentZone { get { return _zones[_curZone]; } }
		public bool isEnd { get { return _isEnd; } }

		protected List<Zone> _zones;
		private string _name;
		private int _curZone;
		private bool _isEnd = false;

		public Level(string name, IEnumerable<Zone> zones = null)
		{
			_name = name;
			if (zones == null)
				_zones = new List<Zone>();
			else
				(_zones = new List<Zone>()).AddRange(zones);
		}

		public Level AddZone(Zone zone)
		{
			_zones.Add(zone);
			return this;
		}

		public Slice NextSlice()
		{
			Slice thisSlice = _zones[_curZone].NextSlice();
			if (thisSlice == null)
			{
				if (_curZone + 1 < _zones.Count)
					return _zones[++_curZone].NextSlice();
				else
				{
					_isEnd = true;
					return null;
				}
			}
			else
				return thisSlice;
		}

		public Zone NextZone()
		{
			if (_curZone >= _zones.Count)
				return null;
			else
				return _zones[_curZone++];
		}
	}
}
