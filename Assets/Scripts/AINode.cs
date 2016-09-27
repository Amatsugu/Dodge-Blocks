using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	public class AINode
	{

		public float distance { get; set; }
		public Vector3 position { get; set; }


		public float GetOffset(Vector3 pos)
		{
			return Vector3.Distance(position, pos);
		}

		public float GetOffset(float x, float y)
		{
			return GetOffset(new Vector3(x,y));
		}
	}
}
