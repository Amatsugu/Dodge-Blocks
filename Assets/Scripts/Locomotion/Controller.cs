using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace LuminousVector
{
	public abstract class Controller : MonoBehaviour
	{

		private Vector3 strafeVector = new Vector3();
		protected Motor _motor;
		protected Vector3 _lastPos;

		// Use this for initialization
		void Start()
		{
			_motor = GetComponent<Motor>();
		}

		void Update()
		{
			if(GameMaster.playerDead)
			{
				if (Input.GetKeyUp(KeyCode.Space))
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}else
				strafeVector = Control(strafeVector);
		}

		protected abstract Vector3 Control(Vector3 strafeVector);
	}
}
