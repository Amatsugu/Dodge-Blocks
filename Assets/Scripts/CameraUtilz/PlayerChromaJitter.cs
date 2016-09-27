using UnityEngine;
using UnityStandardAssets.ImageEffects;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System;

namespace LuminousVector
{
	[RequireComponent(typeof(PlayerCameraShake))]
	public class PlayerChromaJitter : PlayerEffect
	{
		public bool chromaJitter = true;
		public float chromaJitterMulti = 10;
		private VignetteAndChromaticAberration _chroma;
		private float _baseChroma;
		private PlayerCameraShake _pShake;

		protected override void Init()
		{
			_chroma = GetComponent<VignetteAndChromaticAberration>();
			chromaJitter = (_chroma != null) ? chromaJitter : false;
			if (!chromaJitter)
				return;
			_baseChroma = _chroma.chromaticAberration;
			_pShake = GetComponent<PlayerCameraShake>();

		}

		void Update()
		{
			if (chromaJitter)
				_chroma.chromaticAberration = _baseChroma + (_pShake.shake.lastShakeOffset.magnitude * chromaJitterMulti);
		}

		protected override void TiggerEffect()
		{

		}
	}
}
