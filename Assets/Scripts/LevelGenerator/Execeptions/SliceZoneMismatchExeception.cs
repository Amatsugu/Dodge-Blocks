using System;
using UnityEngine;

namespace LuminousVector.LevelGenerator.Execeptions
{
	public class SliceZoneMismatchExeception : Exception
	{
		public SliceZoneMismatchExeception(Vector2 slice, Vector3 zone) : base ("The size of the slice does not match the size of the zone " + slice.x + "x" + slice.y + " != " + zone.x + "x" + zone.y)
		{

		}
	}
}
