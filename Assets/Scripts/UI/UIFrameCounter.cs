using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace LuminousVector
{
	[RequireComponent(typeof(Text))]
	public class UIFrameCounter : MonoBehaviour 
	{
		public float fps = 0f;
		public float updateRate = 4f;  // 4 updates per sec.

		private float deltaTime = 0f;
		private Text text;

		void Start()
		{
			text = GetComponent<Text>();
		}

		void Update()
		{
			deltaTime += Time.deltaTime;
			deltaTime /= 2f;
			fps = 1f / deltaTime;
			text.text = Utils.Round(fps, 100) + " fps";
		}
	}
}
