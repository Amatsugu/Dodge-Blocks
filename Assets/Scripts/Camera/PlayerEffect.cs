using UnityEngine;
using System.Collections;

namespace LuminousVector
{
	public abstract class PlayerEffect : MonoBehaviour
	{
		public GameEvent shakeEvent;

		void Start()
		{
			EventManager.StartListening(shakeEvent, TiggerEffect);
			Init();
		}

		protected abstract void Init();

		protected abstract void TiggerEffect();
	}
}
