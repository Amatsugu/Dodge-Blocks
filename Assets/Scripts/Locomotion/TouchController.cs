using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace LuminousVector
{
	[RequireComponent(typeof(Motor))]
	public class TouchController : Controller
	{
		protected override Vector3 Control(Vector3 strafeVector)
		{
			Touch touch = Input.GetTouch(0);
			strafeVector = touch.deltaPosition;
			return strafeVector;
		}
	}
}
