using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LuminousVector
{
	[RequireComponent(typeof(Rigidbody))]
	public class AdvancedMotor : MonoBehaviour
	{
		public float startSpeed = 25;
		public float acceleration = 2;
		public float strafeSpeed = 2;
		public float turnSpeed = 2;
		public float sheildRegenRate = 1;
		public float scorePenalty = 500;
		[Range(0, 1)]
		public float velLossMulti = .75f;
		public float velRecoveryRate = 1f;


		[HideInInspector]
		public Vector3 strafeVector;
		[HideInInspector]
		public float strafeProgress;
		[HideInInspector]
		public Vector3 curPos;
		public Vector3 basePos;
		[HideInInspector]
		public float curVel { get { return _curVel; } }

		private Transform _transform;
		private Rigidbody _rigidBody;
		private float _velMulti = 1;
		private float _curVel = 0;
		[SerializeField]
		private float _curSheild = 2;
		private Vector3 _desiredVector;
		[SerializeField]
		private bool _isDead = false;
		private Vector3 _fwd;
		private Vector3 _up;
		private float _turnProgress;

		// Use this for initialization
		void Start()
		{
			_transform = transform;
			_rigidBody = GetComponent<Rigidbody>();
			curPos = basePos = _transform.position;
			_curVel = startSpeed;
		}

		// Update is called once per frame
		void Update()
		{
			if (_isDead)
				return;
			//Progess animation
			//Strafe
			if (strafeProgress < 1)
				strafeProgress += strafeSpeed * Time.deltaTime;
			if (strafeProgress > 1)
				strafeProgress = 1;
			//Turn
			if (_turnProgress < 1)
				_turnProgress += turnSpeed * Time.deltaTime;
			if (_turnProgress > 1)
				_turnProgress = 1;

			//Apply Rotation
			if (_turnProgress != 1)
				_transform.rotation = Quaternion.Lerp(_transform.rotation, Quaternion.LookRotation(_fwd, _up), _turnProgress);


			//Apply animation
			_desiredVector = basePos + strafeVector;
			if(_fwd == Vector3.forward || _fwd == Vector3.back)
				_desiredVector.z = curPos.z;
			else if (_fwd == Vector3.left || _fwd == Vector3.right)
				_desiredVector.x = curPos.x;
			else if (_fwd == Vector3.up || _fwd == Vector3.down)
				_desiredVector.y = curPos.y;
			curPos = Vector3.Lerp(curPos, _desiredVector, strafeProgress);

			//Apply forward motion
			curPos += _fwd * (_curVel * _velMulti) * Time.deltaTime;

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

		void Turn(Vector3 fwd, Vector3 up)
		{
			_fwd = fwd.normalized;
			_up = up.normalized;
			_turnProgress = 0;
		}

		void OnTriggerEnter(Collider c)
		{
			if (c.tag != "obstacle")
				return;
			c.gameObject.SendMessage("Die");
			if (_isDead)
				return;
			//c.gameObject.SetActive(false);
			EventManager.TriggerEvent(GameEvent.PLAYER_CRASH);
			_velMulti = 1 - velLossMulti;
			_curSheild = ((int)_curSheild) - 1;
			GameMaster.score -= scorePenalty;
			if (_curSheild < 0)
				Die();
		}

		void Die()
		{
			GameMaster.playerDead = _isDead = true;
			_rigidBody.isKinematic = false;
			_rigidBody.useGravity = true;
			_rigidBody.velocity = _transform.forward * _curVel;
			EventManager.TriggerEvent(GameEvent.PLAYER_DIE);

		}
	}
}
