using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	public struct GenPoints
	{
		public Vector3 pos { get; set; }
		public Vector3 up { get; set; }
		public Color col { get; set; }

		public GenPoints(Vector3 pos, Vector3 up)
		{
			this.pos = pos;
			this.up = up;
			col = Color.red;
		}

		public GenPoints(Vector3 pos, Vector3 up, Color col)
		{
			this.pos = pos;
			this.up = up;
			this.col = col;
		}
	}
}
