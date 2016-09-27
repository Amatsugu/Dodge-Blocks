using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	public class Controller : MonoBehaviour
	{

		protected Vector3 strafeVector = new Vector3();
		protected Motor _motor;
		protected Vector3 _lastPos;

		// Use this for initialization
		void Start()
		{
			_motor = GetComponent<Motor>();
		}
	}
}
