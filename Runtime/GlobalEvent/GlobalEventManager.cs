using System;
using Hextant;

namespace FinTOKMAK.EventSystem.Runtime.GlobalEvent
{
    public class GlobalEventManager: UniversalEventManager
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
        
        public override UniversalEventConfig GetEventConfig()
        {
            return Settings<GlobalEventSettings>.instance.universalEventConfig;
        }

        protected override void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            
            base.Awake();
            
            DontDestroyOnLoad(this);
            Instance = this;
            finishInitializeEvent?.Invoke();
            initialized = true;
        }
    }
}