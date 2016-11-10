using UnityEngine;

namespace LuminousVector.Uitls
{
	public struct ValueRange
	{
		public float min { get; set; }
		public float max { get; set; }

		public ValueRange(float min, float max)
		{
			this.min = min;
			this.max = max;
		}


		public float RandomValue()
		{
			return Random.Range(min, max); 
		}

		public int RandomValueInt()
		{
			return (int)Random.Range(min, max);
		}

		public float RandomValue(System.Random random)
		{
			return min + ((float)random.NextDouble() * (max - min));
		}

		public int RandomValueInt(System.Random random)
		{
			return (int)RandomValue(random);
		}

		public float Lerp(float t)
		{
			return Mathf.Lerp(min, max, t);
		}

	}
}
