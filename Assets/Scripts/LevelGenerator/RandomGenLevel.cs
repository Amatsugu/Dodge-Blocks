using System;
using LuminousVector.Uitls;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LuminousVector.LevelGenerator
{
	public class RandomGenLevel : Level
	{
		public ValueRange length { get; set; }
		public ValueRange turnFequency { get; set; }
		public int density { get; set; }
		public int width { get; set; }
		public int height { get; set; }
		public string poolID { get; set; }

		private Random _rand;
		private int _lastTurn = 0;
		private int _nextTurn = -1;
		private Slice _emptySlice;

		public RandomGenLevel(string name, int seed = 0) : base(name)
		{
			if (seed != 0)
			{
				_rand = new Random(seed);
			}
			else
				_rand = new Random();

			
		}

		public RandomGenLevel Init()
		{

			List<Voxel> voxels = new List<Voxel>(new Voxel[]
			{
				null, null, null,
				null, null, null,
				null, null, new Voxel(poolID)
			});
			_emptySlice = new Slice(3, 3, voxels);
			return this;
		}

		public RandomGenLevel GenerateZone()
		{
			if (_nextTurn == -1)
				_nextTurn = turnFequency.RandomValueInt(_rand);
			if(_lastTurn >= _nextTurn)
			{
				_nextTurn = turnFequency.RandomValueInt(_rand);
				return GenerateTurn();
			}
			Zone zone = new Zone(width, height);
			//List<Voxel> voxels = new List<Voxel>(9);
			for(int z = 0; z <= length.RandomValue(_rand); z++)
			{
				//voxels.Clear();
				/*for(int y = 0; y < 3; y++)
				{
					for(int x = 0; x < 3; x++)
					{
						if (x == 2 && y == 2)
							voxels.Add(new Voxel(poolID));
						else if (rand.Next(density) != 1)
							voxels.Add(null);
						else
							voxels.Add(new Voxel(poolID));
					}
				}*/
				zone.AddSlice(_emptySlice);//new Slice(3, 3, voxels));
			}
			AddZone(zone);
			_lastTurn++;
			return this;
		}

		RandomGenLevel GenerateTurn()
		{
			TurnDir dir = (TurnDir)_rand.Next(4);
			TurnZone zone = new TurnZone(3, 3, 3, dir);
			_lastTurn = 0;
			AddZone(zone);
			return this;
		}

		public override Zone NextZone()
		{
			GenerateZone();
			return base.NextZone();
		}
	}
}
