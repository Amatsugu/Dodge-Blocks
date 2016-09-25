using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LuminousVector
{
	public class UISpeed : MonoBehaviour
	{

		private Text _text;

		void Start()
		{
			_text = GetComponent<Text>();
		}

		void Update()
		{
			_text.text = Utils.Round(GameMaster.speed, 100) + "m/s";
		}
	}
}
