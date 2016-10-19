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

		private Random rand;
		private int lastTurn = 0;
		private int nextTurn = -1;

		public RandomGenLevel(string name, int seed = 0) : base(name)
		{
			if (seed != 0)
			{
				rand = new Random(seed);
			}
			else
				rand = new Random();
		}

		public RandomGenLevel GenerateZone()
		{
			if (nextTurn == -1)
				nextTurn = turnFequency.RandomValueInt(rand);
			if(lastTurn >= nextTurn)
			{
				nextTurn = turnFequency.RandomValueInt(rand);
				return GenerateTurn();
			}
			Zone zone = new Zone(width, height);
			List<Voxel> voxels = new List<Voxel>(9);
			for(int z = 0; z <= length.RandomValue(rand); z++)
			{
				voxels.Clear();
				for(int y = 0; y < 3; y++)
				{
					for(int x = 0; x < 3; x++)
					{
						if (rand.Next(density) != 1)
							voxels.Add(null);
						else
							voxels.Add(new Voxel(poolID));
					}
				}
				zone.AddSlice(new Slice(3, 3, voxels));
			}
			AddZone(zone);
			lastTurn++;
			return this;
		}

		RandomGenLevel GenerateTurn()
		{
			TurnDir dir = (TurnDir)rand.Next(3);
			int len = 0;
			if (dir == TurnDir.Down || dir == TurnDir.Up)
				len = 4;
			else
				len = 3;
			TurnZone zone = new TurnZone(3, 3, len, dir);
			lastTurn = 0;
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
