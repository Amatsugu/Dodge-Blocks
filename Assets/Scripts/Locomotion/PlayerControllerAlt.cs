using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	[RequireComponent(typeof(Motor))]
	public class PlayerControllerAlt : Controller
	{
		// Update is called once per frame
		protected override void Control()
		{
			//Vertical
			if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
				strafeVector.y += 1;
			else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
				strafeVector.y -= 1;


			//Horizontal
			if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
				strafeVector.x += 1;
			else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
				strafeVector.x -= 1;

			//Clamp
			strafeVector.y = Mathf.Clamp(strafeVector.y, -1, 1);
			strafeVector.x = Mathf.Clamp(strafeVector.x, -1, 1);

			//Return to zero
			if (Input.GetKeyDown(KeyCode.LeftControl))
				strafeVector = Vector3.zero;

			//Reset Animation progress
			if (strafeVector != _lastPos)
				_motor.lerpProgress = 0;

			 _lastPos = _motor.strafeVector = strafeVector;
		}
	}
}
