using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections;
using System;

namespace LuminousVector
{
	public class PlayerBlurJitter : PlayerEffect
	{
		public bool blurJitter = true;
		public float blurJitterMulti = 10;
		
		private CameraMotionBlur _blur;
		private float _baseBlur;
		private PlayerCameraShake _pShake;

		protected override void Init()
		{
			_blur = GetComponent<CameraMotionBlur>();
			blurJitter = (_blur != null) ? blurJitter : false;
			if (!blurJitter)
				return;

			_pShake = GetComponent<PlayerCameraShake>();
			_baseBlur = _blur.velocityScale;
		}

		void Upate()
		{
			if (blurJitter)
				_blur.velocityScale = _baseBlur + (Mathf.Abs(_pShake.shake.lastShakeOffset.x) * blurJitterMulti);
		}

		protected override void TiggerEffect()
		{
			
		}

	}
}
