using UnityEngine;
using System.Collections.Generic;
namespace LuminousVector
{
	public enum Direction
	{
		Horizontal,
		Vertial
	}

	[ExecuteInEditMode]
	public class UIList : MonoBehaviour
	{
		//Public
		public List<RectTransform> elements;
		public Direction direction;
		public float spacing;
		public bool alignChildren = true;
		public bool alignOnStart;
		public bool invertDirection;
		public bool center;
		public bool centerVertical;
		void Start ()
		{
			if(alignOnStart)
				Align();
		}

		void Update()
		{
			if (Application.isPlaying)
				return;
			if (alignChildren)
			{
				elements.Clear();
				RectTransform[] el = GetComponentsInChildren<RectTransform>();
				foreach(RectTransform e in el)
				{
					if (e.parent == transform && e.gameObject.activeInHierarchy)
						elements.Add(e);
				}
			}
			if (elements == null)
				return;
			Align();
		}

		void Align()
		{
			float totalSize = 0, curWidth, curHeight;
			foreach (RectTransform e in elements)
				totalSize += (direction == Direction.Horizontal) ? e.rect.width : e.rect.height;
			float lastPos = (center) ? -(totalSize/2) : 0;
			Vector2 pos;
			foreach (RectTransform e in elements)
			{
				pos = Vector2.zero;
				if (direction == Direction.Horizontal)
				{
					curWidth = e.rect.width * e.localScale.x; //elements[i].rect.width * elements[i].localScale.x;
					pos.x = (invertDirection) ? -lastPos : lastPos;
					lastPos += (curWidth + spacing);
					//if(centerVertical)
					//	pos.y -= (e.rect.height / 2);
				}
				else
				{
					curHeight = e.rect.height * e.localScale.y; //elements[i].rect.height * elements[i].localScale.y;
					pos.y = (invertDirection) ? -lastPos : lastPos;
					lastPos -= (curHeight + spacing);
					//if(centerVertical)
					//	pos.x += (e.rect.width / 2);
				}
				e.localPosition = pos;
			}
		}
	}
}
