using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	[ExecuteInEditMode]
	public class GenerateShatterCube : MonoBehaviour
	{
		public GameObject cube;
		[Range(1,10)]
		public int cubeDensity = 5;
		public List<GameObject> cubes = new List<GameObject>();
		public bool reset = false;
		private Vector3 _curPos = Vector3.zero;


		void Update()
		{
			if (cube == null)
				return;
			if (Application.isPlaying && !reset)
				return;
			foreach (GameObject g in cubes)
				DestroyImmediate(g);
			if (cubes.Count != 0)
				cubes.Clear();
			float scale = 1f / cubeDensity;
			for (_curPos.y = -.5f + scale/2; _curPos.y < .5f; _curPos.y += scale)
			{
				for (_curPos.z = -.5f + scale/2; _curPos.z < .5f; _curPos.z += scale)
				{
					for (_curPos.x = -.5f + scale/2; _curPos.x < .5f; _curPos.x += scale)
					{
						GameObject curCube = (Instantiate(cube, transform.position + _curPos, Quaternion.identity, transform) as GameObject);
						curCube.transform.localScale = new Vector3(scale, scale, scale);
						cubes.Add(curCube);
					}
				}
			}
			reset = false;
		}
	}
}
