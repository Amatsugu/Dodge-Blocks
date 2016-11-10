using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public class Test : MonoBehaviour 
	{
		public bool left, right, up, down;
		public Vector3 dUp = Vector3.up, dRight = Vector3.right, dFwd = Vector3.forward;


		void Update()
		{
			Debug.DrawLine(Vector3.zero, dFwd * 5, Color.blue);
			Debug.DrawLine(Vector3.zero, dRight * 5, Color.red);
			Debug.DrawLine(Vector3.zero, dUp * 5, Color.green);
			if (up)
			{
				dUp = Utils.Rotate(dUp, Mathf.PI / -2, RotationAxis.X);
				dRight = Utils.Rotate(dRight, Mathf.PI / -2, RotationAxis.X);
				dFwd = Utils.Rotate(dFwd, Mathf.PI / -2, RotationAxis.X);
			}
			else if(down)
			{
				dUp = Utils.Rotate(dUp, Mathf.PI / 2, RotationAxis.X);
				dRight = Utils.Rotate(dRight, Mathf.PI / 2, RotationAxis.X);
				dFwd = Utils.Rotate(dFwd, Mathf.PI / 2, RotationAxis.X);
			}
			else if (left)
			{
				dUp = Utils.Rotate(dUp, Mathf.PI / -2, RotationAxis.Y);
				dRight = Utils.Rotate(dRight, Mathf.PI / -2, RotationAxis.Y);
				dFwd = Utils.Rotate(dFwd, Mathf.PI / -2, RotationAxis.Y);
			}
			else if (right)
			{
				dUp = Utils.Rotate(dUp, Mathf.PI / 2, RotationAxis.Y);
				dRight = Utils.Rotate(dRight, Mathf.PI / 2, RotationAxis.Y);
				dFwd = Utils.Rotate(dFwd, Mathf.PI / 2, RotationAxis.Y);
			}
			left = right = up = down = false;
		}
	}
}
