using System;
using System.Collections;
using System.Collections.Generic;
using Hextant;
using NaughtyAttributes;
using Package.Editor;
using UnityEngine;

namespace FinTOKMAK.GlobalEventSystem.Runtime
{
    /// <summary>
    /// This MonoBehaviour provides global event management
    /// </summary>
    public class GlobalEventManager : MonoBehaviour
    {
        #region Singleton

        /// <summary>
        /// The singleton of GlobalEventConfig
        /// </summary>
        public static GlobalEventManager Instance;
        
        /// <summary>
        /// If the GlobalEventManager is initialized
        /// </summary>
        public static bool initialized = false;

        /// <summary>
        /// Call this event when GlobalEventManager finish initialization
        /// </summary>
        public static Action finishInitializeEvent;

        #endregion
        
        #region Public Field
        
        

        #endregion

        #region Private Field

        private GlobalEventConfig _config;
        
        /// <summary>
        /// The dictionary for the system to call the global event
        /// </summary>
        private Dictionary<string, Action<IGlobalEventData>> _eventTable =
            new Dictionary<string, Action<IGlobalEventData>>();

        #endregion

        private void Awake()
        {
            _config =  Settings<GlobalEventSettings>.instance.globalEventConfig;;
            
            Instance = this;
            
            InitializeEventTable();
            
            finishInitializeEvent?.Invoke();
            initialized = true;
        }

        #region Private Field

        /// <summary>
        /// Call this method to initialize the event table.
        /// Load the table with all the event names
        /// </summary>
        private void InitializeEventTable()
        {
            foreach (string eventName in _config.eventNames)
            {
                _eventTable.Add(eventName, data => {});
            }
        }

        #endregion

        #region Public Field

        /// <summary>
        /// Call this method to invoke a global event with certain data
        /// </summary>
        /// <param name="eventName">the name of the event</param>
        /// <param name="data">the event data to pass in</param>
        public void InvokeEvent(string eventName, IGlobalEventData data)
        {
            _eventTable[eventName]?.Invoke(data);
        }

        /// <summary>
        /// Register a method into the event
        /// </summary>
        /// <param name="eventName">the target event name</param>
        /// <param name="registerEvent">the register method or logic</param>
        public void RegisterEvent(string eventName, Action<IGlobalEventData> registerEvent)
        {
            _eventTable[eventName] += registerEvent;
        }

        #endregion
    }
}
