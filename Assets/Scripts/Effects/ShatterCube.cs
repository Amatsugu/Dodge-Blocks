using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public class ShatterCube : MonoBehaviour
	{
		[Range(1,10)]
		public int cubeDensity = 5;
		[Range(0,1)]
		public float inheritVelocity = .5f;
		public float explosionMulti = 1000;
		public GameObject microCube;
		public float lifeTime = 1;

		private List<GameObject> _cubes = new List<GameObject>();
		private Vector3 _curPos = Vector3.zero;
		private float _endTime = 0;

		void Awake()
		{
			EventManager.StartListening(GameEvent.GENERATOR_LOOP, OnGeneratorLoop);
			Vector3 scale = new Vector3(1f / cubeDensity, 1f / cubeDensity, 1f / cubeDensity);
			GameObject curCube;
			for (_curPos.y = -.5f + scale.x / 2; _curPos.y < .5f; _curPos.y += scale.x)
			{
				for (_curPos.z = -.5f + scale.x / 2; _curPos.z < .5f; _curPos.z += scale.x)
				{
					for (_curPos.x = -.5f + scale.x / 2; _curPos.x < .5f; _curPos.x += scale.x)
					{
						curCube = Instantiate(microCube, transform.position + _curPos, Quaternion.identity, transform) as GameObject;
						curCube.transform.localScale = scale;
						_cubes.Add(curCube);
					}
				}
			}
		}

		void Update()
		{
			foreach (GameObject obj in _cubes)
			{
				if (!obj.activeInHierarchy)
					continue;
			}
			if (GameMaster.playerDead)
				return;
			if (Time.time >= _endTime)
				gameObject.SetActive(false);
		}

		void OnGeneratorLoop()
		{
			transform.Translate(0, 0, -GameMaster.loopOffset);
		}

		void OnEnable()
		{
			Rigidbody rbody;
			foreach (GameObject obj in _cubes)
			{
				Debug.DrawLine(transform.position, obj.transform.position, Color.cyan);
				rbody = obj.GetComponent<Rigidbody>();
				rbody.angularVelocity = rbody.velocity = Vector3.zero;
				obj.GetComponent<Rigidbody>().AddForce(Vector3.forward * GameMaster.speed * inheritVelocity * explosionMulti);//.AddExplosionForce(GameMaster.speed * inheritVelocity, transform.position, 5);
			}
			_endTime = Time.time + lifeTime;

		}

		void OnDisable()
		{
			Reset();
		}

		public void Reset()
		{
			if (_cubes.Count == 0)
				return;
			float scale = 1f / cubeDensity;
			int i = 0;
			for (_curPos.y = -.5f + scale / 2; _curPos.y < .5f; _curPos.y += scale)
			{
				for (_curPos.z = -.5f + scale / 2; _curPos.z < .5f; _curPos.z += scale)
				{
					for (_curPos.x = -.5f + scale / 2; _curPos.x < .5f; _curPos.x += scale)
					{
						_cubes[i].transform.rotation = Quaternion.identity;
						_cubes[i].transform.localPosition = _curPos;
						i++;
					}
				}
			}
		}
	}
}
