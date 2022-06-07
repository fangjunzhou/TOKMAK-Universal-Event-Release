using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace FinTOKMAK.EventSystem.Runtime
{
    public delegate Task AsyncAction(IEventData data);
    
    public class AsyncUniversalEventManager : MonoBehaviour
    {
        #region Private Field

        private UniversalEventConfig _config;
        
        /// <summary>
        /// The dictionary for the system to call the event
        /// </summary>
        private Dictionary<string, AsyncAction> _eventTable =
            new Dictionary<string, AsyncAction>();

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
                _eventTable.Add(eventName, data => { return Task.CompletedTask; });
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
        /// Call this method to invoke a event with certain data
        /// </summary>
        /// <param name="eventName">the name of the event</param>
        /// <param name="data">the event data to pass in</param>
        public Task InvokeEvent(string eventName, IEventData data)
        {
            List<Task> res = new List<Task>();
            
            foreach (var @delegate in _eventTable[eventName].GetInvocationList())
            {
                var callback = (AsyncAction) @delegate;
                res.Add(callback(new EventData()));
            }
            
            return Task.WhenAll(res);
        }

        /// <summary>
        /// Register a method into the event
        /// </summary>
        /// <param name="eventName">the target event name</param>
        /// <param name="registerEvent">the register method or logic</param>
        public void RegisterEvent(string eventName, AsyncAction registerEvent)
        {
            _eventTable[eventName] += registerEvent;
        }

        /// <summary>
        /// Unregister a method from the event
        /// </summary>
        /// <param name="eventName">the target event name</param>
        /// <param name="registerEvent">the register method or logic</param>
        public void UnRegisterEvent(string eventName, AsyncAction registerEvent)
        {
            _eventTable[eventName] -= registerEvent;
        }

        #endregion
    }
}