using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	public class CameraShake
	{
		[SerializeField]
		public float ammount = 1;
		[SerializeField]
		public float decay = 5;
		[SerializeField]
		public float intensity = 1;

		public Vector3 lastShakeOffset
		{
			get { return _lastShakeValue; }
		}

		public Vector3 shakeOffset
		{
			get { return GetShake();}
		}

		private Vector3 _lastShakeValue = Vector3.zero;

		private Vector3 _curPos = new Vector3();
		private float _intensity;


		public CameraShake StartShake()
		{
			_intensity = intensity;
			return this;
		}

		public Vector3 GetShake()
		{
			if (_intensity > 0)
			{
				float x = (Mathf.PerlinNoise(Time.time, Mathf.Sin(Time.time)) - 0.5f) * ammount * 2;
				float y = (Mathf.PerlinNoise(Time.time, Mathf.Cos(Time.time)) - 0.5f) * ammount * 2;
				_curPos = new Vector3(Mathf.Clamp(x * _intensity, -ammount, ammount), Mathf.Clamp(y * _intensity, -ammount, ammount), 0);
				//Debug.Log(_curPos + "|" + _intensity);
				_intensity -= decay * Time.deltaTime;
			}else if (_intensity == 0)
				_curPos = Vector3.zero;
			else
				_intensity = 0;

			return _lastShakeValue = _curPos;
		}
	}
}
