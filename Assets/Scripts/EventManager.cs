using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;

namespace LuminousVector
{
	public class EventManager : MonoBehaviour
	{
		private Dictionary<GameEvent, UnityEvent> eventDictionary;

		private static EventManager EVENT_MANAGER;

		public static EventManager instance
		{
			get
			{
				if (!EVENT_MANAGER)
				{
					EVENT_MANAGER = FindObjectOfType<EventManager>() as EventManager;
					if (!EVENT_MANAGER)
					{
						Debug.LogError("No Event Manager found");
					}
					else
					{
						EVENT_MANAGER.Init();
					}
				}
				return EVENT_MANAGER;
			}
		}

		void OnLevelLoaded(int level)
		{
			if(eventDictionary != null)
				eventDictionary.Clear();
		}

		void Start()
		{
			DontDestroyOnLoad(gameObject);
			if (FindObjectOfType<EventManager>() as EventManager != this)
				Destroy(gameObject);
		}

		void Init()
		{
			if (eventDictionary == null)
			{
				eventDictionary = new Dictionary<GameEvent, UnityEvent>();
			}
		}

		public static void StartListening(GameEvent @event, UnityAction listener)
		{
			UnityEvent thisEvent = null;
			if (instance.eventDictionary.TryGetValue(@event, out thisEvent))
			{
				thisEvent.AddListener(listener);
			}
			else
			{
				thisEvent = new UnityEvent();
				thisEvent.AddListener(listener);
				instance.eventDictionary.Add(@event, thisEvent);
			}
		}

		public static void StopListening(GameEvent @event, UnityAction listener)
		{
			if (EVENT_MANAGER == null)
				return;
			UnityEvent thisEvent = null;
			if (instance.eventDictionary.TryGetValue(@event, out thisEvent))
			{
				thisEvent.RemoveListener(listener);
			}
		}

		public static void TriggerEvent(GameEvent @event)
		{
			UnityEvent thisEvent = null;
			if (instance.eventDictionary.TryGetValue(@event, out thisEvent))
			{
				thisEvent.Invoke();
			}
		}
	}
}
