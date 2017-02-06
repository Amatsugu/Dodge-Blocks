using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LuminousVector.LevelGenerator
{
	class TurnHelper
	{
		internal static int TurnUp(PlaneAxis plane, Vector3 fwd, Vector3 up, out RotationAxis axis)
		{
			if (plane == PlaneAxis.XZ || plane == PlaneAxis.XY)
			{
				axis = RotationAxis.X;
				if (fwd != Vector3.back && up == Vector3.down)
					return -1;
				else if (fwd == Vector3.back && up == Vector3.up)
					return -1;
				else if (fwd == Vector3.up && up == Vector3.forward)
					return -1;
				else if (fwd == Vector3.down && up == Vector3.back)
					return -1;
				else return 1;

			}
			else if (plane == PlaneAxis.YX || plane == PlaneAxis.YZ)
			{
				axis = RotationAxis.Y;
				if (fwd == Vector3.forward && up == Vector3.right)
					return -1;
				else if (fwd == Vector3.left && up == Vector3.forward)
					return -1;
				else if (fwd == Vector3.right && up == Vector3.back)
					return -1;
				else if (fwd == Vector3.back && up == Vector3.left)
					return -1;
				else return 1;

			}
			else
			{
				axis = RotationAxis.Z;
				if (fwd == Vector3.down && up == Vector3.right)
					return -1;
				else if (fwd == Vector3.left && up == Vector3.down)
					return -1;
				else if (fwd == Vector3.right && up == Vector3.up)
					return -1;
				else if (fwd == Vector3.up && up == Vector3.left)
					return -1;
				else return 1;

			}
		}

		internal static int TurnDown(PlaneAxis plane, Vector3 fwd, Vector3 up, out RotationAxis axis)
		{
			if (plane == PlaneAxis.XZ || plane == PlaneAxis.XY)
			{
				axis = RotationAxis.X;
				if (fwd == Vector3.back && up == Vector3.up)
					return -1;
				else if (fwd == Vector3.forward && up == Vector3.down)
					return -1;
				else if (fwd == Vector3.down && up == Vector3.back)
					return -1;
				else if (fwd == Vector3.up && up == Vector3.forward)
					return -1;
				else
					return 1;
			}
			else if (plane == PlaneAxis.YX || plane == PlaneAxis.YZ)
			{
				axis = RotationAxis.Y;
				if (fwd == Vector3.left && up == Vector3.forward)
					return -1;
				else if (fwd == Vector3.right && up == Vector3.back)
					return -1;
				else if (fwd == Vector3.back && up == Vector3.left)
					return -1;
				else if (fwd == Vector3.forward && up == Vector3.right)
					return -1;
				else
					return 1;
			}
			else
			{
				axis = RotationAxis.Z;
				if (fwd == Vector3.right && up == Vector3.up)
					return -1;
				else if (fwd == Vector3.left && up == Vector3.down)
					return -1;
				else if (fwd == Vector3.down && up == Vector3.right)
					return -1;
				else if (fwd == Vector3.up && up == Vector3.left)
					return -1;
				else
					return 1;
			}
		}

		internal static int TurnLeft(PlaneAxis plane, Vector3 fwd, out RotationAxis axis)
		{
			if (plane == PlaneAxis.XZ || plane == PlaneAxis.ZX)
			{
				axis = RotationAxis.Y;
				if (fwd == Vector3.down)
					return -1;
				else
					return 1;
			}
			else if (plane == PlaneAxis.XY || plane == PlaneAxis.YX)
			{
				axis = RotationAxis.Z;
				if (fwd == Vector3.back)
					return -1;
				else
					return 1;
			}
			else
			{
				axis = RotationAxis.X;
				if (fwd == Vector3.left)
					return -1;
				else
					return 1;

			}
		}

		internal static int TurnRight(PlaneAxis plane, Vector3 up, out RotationAxis axis)
		{
			if (plane == PlaneAxis.XZ || plane == PlaneAxis.ZX)
			{
				axis = RotationAxis.Y;
				if (up == Vector3.down)
					return -1;
				else
					return 1;
			}
			else if (plane == PlaneAxis.XY || plane == PlaneAxis.YX)
			{
				axis = RotationAxis.Z;
				if (up == Vector3.back)
					return -1;
				else
					return 1;
			}
			else
			{
				axis = RotationAxis.X;
				if (up == Vector3.left)
					return -1;
				else
					return 1;
			}
		}
	}
}
