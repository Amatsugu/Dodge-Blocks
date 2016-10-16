using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	[RequireComponent(typeof(Motor))]
	public class PlayerController : Controller
	{
		// Update is called once per frame
		protected override void Control()
		{
			//Vertical
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
				strafeVector.y = 1;
			else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
				strafeVector.y = -1;
			else
				strafeVector.y = 0;

			//Horizontal
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
				strafeVector.x = 1;
			else if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
				strafeVector.x = -1;
			else
				strafeVector.x = 0;

			//Reset Animation progress
			if (strafeVector != _lastPos)
				_motor.lerpProgress = 0;

			 _lastPos = _motor.strafeVector = strafeVector;
		}
	}
}
