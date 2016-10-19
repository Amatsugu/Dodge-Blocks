using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace LuminousVector
{
	[ExecuteInEditMode]
	public class UIBlurBehind : MonoBehaviour 
	{
		private RectTransform _thisRectTrans;
		private int _id = -1;

		void Start()
		{
			_thisRectTrans = GetComponent<RectTransform>();
		}

		void Update()
		{
			Rect rect = new Rect();
			rect.position = new Vector2(_thisRectTrans.position.x / Screen.width, _thisRectTrans.position.y / Screen.height);
			rect.size = new Vector2(_thisRectTrans.rect.width / Screen.width, _thisRectTrans.rect.height / Screen.height);
			_id = UIBlurBehindController.AddBlurRegion(rect, _id);
		}
	}
}
