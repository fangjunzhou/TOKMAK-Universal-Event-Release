using System;
using System.Collections;
using System.Collections.Generic;
using Hextant;
using NaughtyAttributes;
using UnityEngine;

namespace FinTOKMAK.EventSystem.Runtime
{
    /// <summary>
    /// This MonoBehaviour provides global event management
    /// </summary>
    public class UniversalEventManager : MonoBehaviour
    {
        
        
        #region Public Field
        
        

        #endregion

        #region Private Field

        private UniversalEventConfig _config;
        
        /// <summary>
        /// The dictionary for the system to call the global event
        /// </summary>
        private Dictionary<string, Action<IEventData>> _eventTable =
            new Dictionary<string, Action<IEventData>>();

        #endregion

        protected virtual void Awake()
        {
            _config = GetEventConfig();

            InitializeEventTable();
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
        /// The method to get the event config for the EventManager. 
        /// </summary>
        /// <returns>Event config of current event set.</returns>
        public virtual UniversalEventConfig GetEventConfig()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Call this method to invoke a global event with certain data
        /// </summary>
        /// <param name="eventName">the name of the event</param>
        /// <param name="data">the event data to pass in</param>
        public void InvokeEvent(string eventName, IEventData data)
        {
            _eventTable[eventName]?.Invoke(data);
        }

        /// <summary>
        /// Register a method into the event
        /// </summary>
        /// <param name="eventName">the target event name</param>
        /// <param name="registerEvent">the register method or logic</param>
        public void RegisterEvent(string eventName, Action<IEventData> registerEvent)
        {
            _eventTable[eventName] += registerEvent;
        }

        /// <summary>
        /// Unregister a method from the event
        /// </summary>
        /// <param name="eventName">the target event name</param>
        /// <param name="registerEvent">the register method or logic</param>
        public void UnRegisterEvent(string eventName, Action<IEventData> registerEvent)
        {
            _eventTable[eventName] -= registerEvent;
        }

        #endregion
    }
}
