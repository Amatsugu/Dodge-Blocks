using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;


namespace LuminousVector
{
	public class CamMove : MonoBehaviour
	{

		public float startSpeed = 25;
		public float acceleration = 2;
		public float strafeSpeed = 2;
		public float sheildRegenRate = 1;
		public float shakeAmmount = 1;
		public float shakeDecay = 5;
		public float shakeIntensity = 5;
		public bool updateShake = true;
		public bool chromaShake = true;
		public float chromaShakeMulti = 10;
		public bool blurShake = true;
		public float blurShakeMulti = 10;
		[Range(0,1)]
		public float velLossMulti = .75f;
		public float velRecoveryRate = 1f;

		private Vector3 _strafeVector = new Vector3();
		private Vector3 _basePos;
		private Transform _transform;
		private float _lerpProgress;
		private CameraShake _shake = new CameraShake();
		private VignetteAndChromaticAberration _chroma;
		private CameraMotionBlur _blur;
		private float _baseChroma;
		private float _baseBlur;
		private float _velMulti = 1;
		private float _curVel = 0;
		private float _curSheild = 2;

		// Use this for initialization
		void Start()
		{
			_shake = new CameraShake()
			{
				ammount = shakeAmmount,
				decay = shakeDecay,
				intensity = shakeIntensity
			};
			_transform = transform;
			_basePos = _transform.position;
			_chroma = GetComponent<VignetteAndChromaticAberration>();
			_blur = GetComponent<CameraMotionBlur>();
			_baseChroma = _chroma.chromaticAberration;
			_baseBlur = _blur.velocityScale;
			_curVel = startSpeed;
		}

		// Update is called once per frame
		void Update()
		{
			if (updateShake)
			{
				_shake = new CameraShake()
				{
					ammount = shakeAmmount,
					decay = shakeDecay,
					intensity = shakeIntensity
				};
				updateShake = false;
			}
			//Vertical
			if (Input.GetKey(KeyCode.W))
				_strafeVector.y = 1;
			else if (Input.GetKey(KeyCode.S))
				_strafeVector.y = -1;
			else
				_strafeVector.y = 0;

			//Horizontal
			if (Input.GetKey(KeyCode.D))
				_strafeVector.x = 1;
			else if (Input.GetKey(KeyCode.A))
				_strafeVector.x = -1;
			else
				_strafeVector.x = 0;

			//Reset Animation progress
			if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
				_lerpProgress = 0;

			//Progess animation
			if (_lerpProgress < 1)
				_lerpProgress += strafeSpeed * Time.deltaTime;
			if (_lerpProgress > 1)
				_lerpProgress = 1;

			//Apply animation
			Vector3 pos = Vector3.Lerp(_transform.position, new Vector3
			{
				x = _basePos.x + _strafeVector.x,
				y = _basePos.y + _strafeVector.y,
				z = transform.position.z
			}, _lerpProgress);

			//Apply forward motion
			pos.z += (_curVel * _velMulti) * Time.deltaTime;


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

			//Apply Shake
			Vector3 shake = _shake.GetShakeOffset();
			Camera.main.rect = new Rect(shake.x, shake.y, Camera.main.rect.width, Camera.main.rect.height);
			if(chromaShake)
				_chroma.chromaticAberration = _baseChroma + (shake.x * chromaShakeMulti);
			if (blurShake)
				_blur.velocityScale = _baseBlur + (Mathf.Abs(shake.x) * blurShakeMulti);
			_transform.position = pos;
		}

		void OnTriggerEnter(Collider c)
		{
			_shake.Shake();
			_velMulti = 1 - velLossMulti;
			_curSheild = ((int)_curSheild) - 1;
			if (_curSheild < 0)
				Die();
		}

		void Die()
		{

		}
	}
}
