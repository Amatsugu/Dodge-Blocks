using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

namespace LuminousVector
{
	public class UIScore : MonoBehaviour
	{
		public UITextFall textFall;

		private Text _text;
		private float _curScore;
		private float _lastScore;



		void Start()
		{
			_text = GetComponent<Text>();
		}

		void Update()
		{
			_curScore = GameMaster.score;
			_text.text = Utils.Round(_curScore, 1).ToString();
			if(_curScore < _lastScore)
			{
				textFall.Fall("-" + Utils.Round(_lastScore - _curScore, 1));
			}
			_lastScore = _curScore;

		}		
	}
}
