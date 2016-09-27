using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LuminousVector
{
	[RequireComponent(typeof(Text))]
	public class UITextFall : MonoBehaviour
	{
		public float duration;
		public AnimationCurve fallCurve = AnimationCurve.Linear(0, 0, 1, 1);
		public AnimationCurve opacityCurve = AnimationCurve.Linear(0, 0, 1, 1);

		private float _endTime;
		private Text _text;
		[SerializeField]
		private Vector3 _pos;
		private Color _color;

		float progress
		{
			get { return Mathf.Clamp((_endTime - Time.time) / duration, 0, 1); }
		}

		void Start()
		{
			_text = GetComponent<Text>();
			_color = _text.color;
			_pos = _text.rectTransform.localPosition;
		}

		void Update()
		{
			_color.a = opacityCurve.Evaluate(progress);
			_pos.y = fallCurve.Evaluate(progress);
			_text.color = _color;
			_text.rectTransform.localPosition = _pos;
		}

		public void Fall(string text)
		{
			_endTime = Time.time + duration;
			_text.text = text;
		}


	}
}
