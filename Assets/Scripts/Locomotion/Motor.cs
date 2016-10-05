using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;


namespace LuminousVector
{
	[RequireComponent(typeof(Rigidbody))]
	public class Motor : MonoBehaviour
	{

		public float startSpeed = 25;
		public float acceleration = 2;
		public float strafeSpeed = 2;
		public float sheildRegenRate = 1;
		public float scorePenalty = 500;
		[Range(0,1)]
		public float velLossMulti = .75f;
		public float velRecoveryRate = 1f;

		[HideInInspector]
		public Vector3 strafeVector;
		[HideInInspector]
		public float lerpProgress;
		[HideInInspector]
		public Vector3 curPos;
		public Vector3 basePos;
		[HideInInspector]
		public float curVel { get { return _curVel; } }
		
		private Transform _transform;
		private float _velMulti = 1;
		private float _curVel = 0;
		private float _curSheild = 2;
		private Vector3 _desiredVector;

		// Use this for initialization
		void Start()
		{
			_transform = transform;
			curPos = basePos = _transform.position;
			_curVel = startSpeed;
		}

		// Update is called once per frame
		void LateUpdate()
		{
			//Progess animation
			if (lerpProgress < 1)
				lerpProgress += strafeSpeed * Time.deltaTime;
			if (lerpProgress > 1)
				lerpProgress = 1;


			//Apply animation
			_desiredVector = basePos + strafeVector;
			_desiredVector.z = curPos.z;
			curPos = Vector3.Lerp(curPos, _desiredVector, lerpProgress);


			//Apply forward motion
			curPos.z += (_curVel * _velMulti) * Time.deltaTime;


			//Acceleration
			_curVel += acceleration * Time.deltaTime;

			//Recover Velocity
			if (_velMulti < 1)
				_velMulti += velRecoveryRate * Time.deltaTime;
			else if (_velMulti > 1)
				_velMulti = 1;

			//Sheild Regen
			if (_curSheild < 2)
				_curSheild += sheildRegenRate * Time.deltaTime;
			if (_curSheild > 2)
				_curSheild = 2;

			//Update Gamemaster
			GameMaster.score += (_curVel * _velMulti) * Time.deltaTime;
			GameMaster.speed = _curVel * _velMulti;
			GameMaster.sheild = _curSheild;

			_transform.position = curPos;
		}

		//Move Player
		public void Loop(float distance)
		{
			curPos.z -= distance;
			_transform.position = curPos;
		}

		void OnCollisionEnter(Collision c)
		{
			Debug.Log(c.collider.tag);
			if (c.collider.tag != "obstacle")
				return;
			c.gameObject.SendMessage("Die");
			EventManager.TriggerEvent(GameEvent.PLAYER_CRASH);
			_velMulti = 1 - velLossMulti;
			_curSheild = ((int)_curSheild) - 1;
			GameMaster.score -= scorePenalty;
			if (_curSheild < 0)
				Die();
		}

		void Die()
		{

		}
	}
}
