using UnityEngine;
using System.Collections.Generic;

namespace LuminousVector
{
	public class GameMaster : MonoBehaviour
	{
		public static float score { get { return instance._score; } set { instance._score = value; } }
		public static float speed { get { return instance._speed; } set { instance._speed = value; } }
		public static float sheild { get { return instance._sheild; } set { instance._sheild = value; } }
		public static float loopOffset { get { return instance._loopOffset; } set { instance._loopOffset = value; } }
		public static bool playerDead { get { return instance._playerDead; } set { instance._playerDead = value; } }

		private static GameMaster GAME_MASTER;

		public static GameMaster instance
		{
			get
			{
				if (!GAME_MASTER)
				{
					GAME_MASTER = FindObjectOfType<GameMaster>();
					if (!GAME_MASTER)
					{
						Debug.LogError("Game master not found");
					}
				}
				return GAME_MASTER;
			}
		}


		private float _score;
		private float _speed;
		private float _sheild;
		private float _loopOffset;
		private bool _playerDead;


	}
}
