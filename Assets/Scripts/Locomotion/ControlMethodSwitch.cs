using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public class ControlMethodSwitch : MonoBehaviour
	{
		public PlayerController standard;
		public PlayerControllerAlt alt;
		public AIController ai;

		public void OnModeChange(int mode)
		{
			if(mode == 0)
			{
				standard.enabled = true;
				alt.enabled = false;
				ai.enabled = false;
			}else if(mode == 1)
			{
				standard.enabled = false;
				alt.enabled = true;
				ai.enabled = false;
			}else
			{
				standard.enabled = false;
				alt.enabled = false;
				ai.enabled = true;
			}
		}
	}
}
