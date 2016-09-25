using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections;

namespace LuminousVector
{
	public class UIScore : MonoBehaviour
	{
		private Text _text;

		void Start()
		{
			_text = GetComponent<Text>();
		}

		void Update()
		{
			_text.text = Utils.Round(GameMaster.score, 100).ToString();
		}		
	}
}
