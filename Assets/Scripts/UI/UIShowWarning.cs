using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace LuminousVector
{
	public class UIShowWarning : MonoBehaviour
	{
		[SerializeField]
		private List<UIFlash> _flash = new List<UIFlash>();

		void Start()
		{
			GetComponentsInChildren(_flash);
		}

		void Update()
		{
			foreach (UIFlash f in _flash)
				f.alphaMulti = (GameMaster.sheild >= 1f) ? 1f - (GameMaster.sheild) : 1f;
		}
	}
}
