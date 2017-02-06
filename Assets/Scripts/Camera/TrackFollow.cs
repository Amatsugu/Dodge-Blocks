using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using LuminousVector.LevelGenerator;

namespace LuminousVector
{
	public class TrackFollow : MonoBehaviour 
	{
		public AdvancedGenerator generator;
		public float offset = 3;
		public bool auto = false;
		public float speed = 5;
		public Text text;
		public Text seed;

		public int _curPoint = 0;
		private int _prevPoint = 0;
		private float _progress = 0;
		private GenPoints _thisPoint { get { return generator._genPoints[_curPoint]; } }
		private GenPoints _lastPoint { get { return generator._genPoints[_prevPoint]; } }
		
		void Start()
		{
			seed.text = "Seed: " + generator.seed;
		}

		void Update()
		{
			if (Input.GetKeyUp(KeyCode.R))
				SceneManager.LoadScene(0);
			if (Input.GetKeyUp(KeyCode.S))
			{
				_prevPoint = _curPoint;
				_curPoint--;
				_progress = 0;
			}
			else if (Input.GetKeyUp(KeyCode.W))
			{
				_prevPoint = _curPoint;
				_curPoint++;
				_progress = 0;
			}
			if (_curPoint < 0)
				_curPoint = _prevPoint = 0;
			if (_curPoint >= generator._genPoints.Count)
				_prevPoint = _curPoint = generator._genPoints.Count - 1;
			transform.rotation = Quaternion.Lerp(Quaternion.LookRotation(_lastPoint.fwd, _lastPoint.up), Quaternion.LookRotation(_thisPoint.fwd, _thisPoint.up), _progress);
			transform.position = Vector3.Lerp(_lastPoint.pos + (_lastPoint.up * offset), _thisPoint.pos + (_thisPoint.up * offset), _progress);
			if (_thisPoint.col == Color.gray)
				text.text = "["+ _curPoint +"] UP";
			else if (_thisPoint.col == Color.yellow)
				text.text = "[" + _curPoint + "] DOWN";
			else if (_thisPoint.col == Color.black)
				text.text = "[" + _curPoint + "] LEFT";
			else if (_thisPoint.col == Color.blue)
				text.text = "[" + _curPoint + "] RIGHT";
			else
				text.text = "[" + _curPoint + "] FWD";
			text.color = _thisPoint.col;
			if (_progress < 1)
				_progress += Time.deltaTime * speed;
			if (_progress > 1)
			{
				if (!auto)
					_progress = 1;
				else
				{
					_progress = 0;
					_prevPoint = _curPoint;
					_curPoint++;
				}
			}


		}
	}
}
