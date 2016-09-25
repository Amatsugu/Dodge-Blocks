using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace LuminousVector
{
	public class Utils : MonoBehaviour
	{
		//Instantiates a UI Image at a specficed position, and with a spefiied parent
		public static Image CreateUIImage(Object obj, Vector2 pos, Transform parent)
		{
			GameObject g = Instantiate(obj, pos, Quaternion.identity) as GameObject;
			g.transform.SetParent(parent, false);
			return g.GetComponent<Image>();
		}

		//Alternate version that outputs the image transform
		public static Image CreateUIImage(Object obj, Vector2 pos, Transform parent, out Transform t)
		{
			GameObject g = Instantiate(obj, pos, Quaternion.identity) as GameObject;
			t = g.transform;
			t.SetParent(parent, false);
			return g.GetComponent<Image>();
		}

		//Converts an integer to a string that includes a specified ammount of preceeding zeros 
		public static string FormatZeros(int value, int zeros)
		{
			string output = value.ToString();
			if(output.Length < zeros)
			{
				for(int i = output.Length; i < zeros; i++)
				{
					output = "0" + output;
				}
			}
			return output;
		}

		public static float Round(float n, float d)
		{
			return ((int)(n * d)) / d;
		}
	}
}
