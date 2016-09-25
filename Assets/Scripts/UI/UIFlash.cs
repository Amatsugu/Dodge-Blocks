using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

namespace LuminousVector
{

	public class UIFlash : MonoBehaviour
	{
		public float flashRate = 1;
		[Range(0,1)]
		public float startProgress = 0;
		public AnimationCurve animCurve = new AnimationCurve();
		public bool flashSelf = true;
		public bool animate = true;
		public float alphaMulti
		{
			get
			{
				return _alphaMulti;
			}
			set
			{
				_alphaMulti = Mathf.Clamp(value, 0, 1);
			}
		}

		
		private float _curProgress;
		private int _direction = 1;
		[SerializeField]
		private float _alphaMulti = 1;
		private List<Image> _images = new List<Image>();
		private List<Text> _text = new List<Text>();
		private List<float> _imgAlpha = new List<float>();
		private List<float> _txtAlpha = new List<float>();

		void Start()
		{
			GetComponentsInChildren(_images);
			GetComponentsInChildren(_text);
			if (!flashSelf)
			{
				if (GetComponent<Image>() != null)
					_images.Remove(GetComponent<Image>());
				if (GetComponent<Text>() != null)
					_text.Remove(GetComponent<Text>());
			}
			_imgAlpha = (from i in _images select i.color.a).ToList();
			_txtAlpha = (from t in _text select t.color.a).ToList();
		}

		void Update()
		{
			if (!animate)
				return;
			for(int i = 0; i < _images.Count; i++)
			{
				Image curImg = _images[i];
				curImg.color = new Color()
				{
					r = curImg.color.r,
					g = curImg.color.g,
					b = curImg.color.b,
					a = (_imgAlpha[i] * animCurve.Evaluate(_curProgress)) * _alphaMulti
				};
			}
			for (int i = 0; i < _text.Count; i++)
			{
				Text curTxt = _text[i];
				curTxt.color = new Color()
				{
					r = curTxt.color.r,
					g = curTxt.color.g,
					b = curTxt.color.b,
					a = (_txtAlpha[i] * animCurve.Evaluate(_curProgress)) * _alphaMulti
				};
			}

			_curProgress += flashRate * _direction * Time.deltaTime;
			if (_curProgress > 1)
				_direction = -1;
			if (_curProgress < 0)
				_direction = 1;
			Mathf.Clamp(_curProgress, 0, 1);
		}
	}
}
