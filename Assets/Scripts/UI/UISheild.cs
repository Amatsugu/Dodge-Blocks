using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LuminousVector
{
	public class UISheild : MonoBehaviour
	{
		[Range(1,2)]
		public int upperRange = 1;

		private Image _sheildBar;

		void Start()
		{
			_sheildBar = GetComponent<Image>();
		}
		
		void Update()
		{

			_sheildBar.color = new Color()
			{
				r = _sheildBar.color.r,
				g = _sheildBar.color.g,
				b = _sheildBar.color.b,
				a = (upperRange == 2) ? GameMaster.sheild - 1 : GameMaster.sheild
			};
		}
	}
}
