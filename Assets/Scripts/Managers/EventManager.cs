using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

namespace Managers
{
    public enum EVENT
    {
        LowWater = 0,
        LowFertilizer = 1,
        Grown = 2,
        Died = 3,
    }

    [Serializable]
    public class GameEvent
    {
        public EVENT handle;
        public string message;
        public Sprite icon;
        public GameObject context;
        public bool dismissable;
        public float duration;

        public GameEvent(EVENT handle, string message, Sprite icon, bool dismissable = false,
            float duration = 5f, GameObject context = null)
        {
            this.handle = handle;
            this.message = message;
            this.icon = icon;
            this.context = context;
            this.dismissable = dismissable;
            this.duration = duration;
        }
        public GameEvent(GameEvent e)
        {
            this.handle = e.handle;
            this.message = e.message;
            this.icon = e.icon;
            this.context = e.context;
            this.dismissable = e.dismissable;
            this.duration = e.duration;
        }

        public string GetSignature()
        {
            return handle.ToString() + context.GetInstanceID();
        }
    }

    public class EventManager : MonoBehaviour
    {
        protected static EventManager instance;
        public static EventManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = (EventManager)FindObjectOfType(typeof(EventManager));

                    if (instance == null)
                    {
                        Debug.LogError("An instance of " + typeof(EventManager) + " is needed in the scene, but there is none.");
                    }
                }
                return instance;
            }
        }
        
        public delegate void OnGameEventCreated(GameEvent gameEvent);
        public event OnGameEventCreated OnGameEventCreatedEvent;

        private readonly Dictionary<EVENT, GameEvent> gameEventDictionary = new Dictionary<EVENT, GameEvent>();
        private Dictionary<string, Action> eventDictionary;

        [SerializeField] private List<GameEvent> gameEvents = new List<GameEvent>();
        [SerializeField] private static readonly float timeBetweenGameEvents = 10;
        [SerializeField] private static readonly float suppressTime = 30;
        private static HashSet<string> gameEventSignatures = new HashSet<string>();
        private static Dictionary<string, float> suppressedEventSignatures = new Dictionary<string, float>();
        private static float gameEventTimer;

        public void Awake()
        {
            foreach (GameEvent gameEvent in gameEvents)
            {
                if (!gameEventDictionary.ContainsKey(gameEvent.handle))
                {
                    gameEventDictionary.Add(gameEvent.handle, gameEvent);
                }
            }
            
            if (eventDictionary == null)
            {
                eventDictionary = new Dictionary<string, Action>();
            }
            

            StartCoroutine(_EventTimer());
        }

        private IEnumerator _EventTimer()
        {
            while (true)
            {
                if (gameEventTimer >= 0)
                {
                    gameEventTimer -= Time.deltaTime;
                }
                else
                {
                    gameEventTimer = timeBetweenGameEvents;
                }

                //Count down the suppressed events
                List<string> keys = new List<string>(suppressedEventSignatures.Keys);
                foreach (string key in keys)
                {
                    suppressedEventSignatures[key] = suppressedEventSignatures[key] - Time.deltaTime;
                    if (suppressedEventSignatures[key] <= 0f)
                    {
                        suppressedEventSignatures.Remove(key);
                    }
                }

                yield return new WaitForSeconds(0.1f);
            }
        }

        public static void StartListening(string eventName, Action listener)
        {
            Action thisEvent;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Add more event to the existing one
                thisEvent += listener;

                //Update the Dictionary
                Instance.eventDictionary[eventName] = thisEvent;
            }
            else
            {
                //Add event to the Dictionary for the first time
                thisEvent += listener;
                Instance.eventDictionary.Add(eventName, thisEvent);
            }
        }
        
        public static void StopListening(string eventName, Action listener)
        {
            if (instance == null) return;
            Action thisEvent;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                //Remove event from the existing one
                thisEvent -= listener;

                //Update the Dictionary
                Instance.eventDictionary[eventName] = thisEvent;
            }
        }

        public static void TriggerEvent(string eventName)
        {
            Action thisEvent = null;
            if (Instance.eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
                // OR USE instance.eventDictionary[eventName]();
            }
        }

        public void CreateGameEvent(EVENT handle, GameObject context)
        {
            if(OnGameEventCreatedEvent == null) return;
            string eventSignature = handle.ToString() + context.GetInstanceID();
            if (!gameEventSignatures.Contains(eventSignature) && !suppressedEventSignatures.ContainsKey(eventSignature))
            {
                GameEvent gameEvent;
                if (gameEventDictionary.TryGetValue(handle, out gameEvent))
                {
                    gameEvent = new GameEvent(gameEvent);
                    gameEvent.context = context;
                    gameEvent.message += " : " + context.name;
                    gameEventSignatures.Add(eventSignature);
                    OnGameEventCreatedEvent.Invoke(gameEvent);
                }
                else
                {
                    Debug.Log("Game event with this handle does not exist");
                }
            }
        }

        public void AcknowledgeEvent(string signature)
        {
            if (gameEventSignatures.Contains(signature))
                gameEventSignatures.Remove(signature);
            if (suppressedEventSignatures.ContainsKey(signature))
            {
                suppressedEventSignatures[signature] = suppressTime;
            }
            else
            {
                suppressedEventSignatures.Add(signature, suppressTime);
            }
        }
    }
}