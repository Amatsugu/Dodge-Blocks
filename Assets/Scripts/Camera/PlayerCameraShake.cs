using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System;

namespace LuminousVector
{
	[RequireComponent(typeof(Camera))]
	public class PlayerCameraShake : PlayerEffect
	{
		public float shakeAmmount = 1;
		public float shakeDecay = 5;
		public float shakeIntensity = 5;
		public bool updateShake = true;
		
		[HideInInspector]
		public CameraShake shake = new CameraShake();

	 	protected override void Init()
		{
			shake = new CameraShake()
			{
				ammount = shakeAmmount,
				decay = shakeDecay,
				intensity = shakeIntensity
			};
		}

		void Update()
		{
			if (updateShake)
			{
				shake.ammount = shakeAmmount;
				shake.decay = shakeDecay;
				shake.intensity = shakeIntensity;
				updateShake = false;
			}

			//Simulate Shake
			shake.Simulate();

			//Apply Shake
			Vector3 pos = Camera.main.transform.localPosition;
			pos.x = shake.lastShakeOffset.x;
			pos.y = shake.lastShakeOffset.y;
			Camera.main.transform.localPosition = pos;
			//Camera.main.rect = new Rect(shake.lastShakeOffset.x, shake.lastShakeOffset.y, Camera.main.rect.width, Camera.main.rect.height);
		}

		protected override void TiggerEffect()
		{
			shake.Shake();
		}
	}

}
