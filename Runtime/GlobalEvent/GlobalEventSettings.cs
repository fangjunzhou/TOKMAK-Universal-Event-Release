using System.Collections.Generic;
using Hextant;
using NaughtyAttributes;
using UnityEngine.Serialization;
#if UNITY_EDITOR
using Hextant.Editor;
using UnityEditor;
#endif

namespace FinTOKMAK.EventSystem.Runtime.GlobalEvent
{
    [System.Serializable]
    public struct SerializedFieldInfo
    {
        public string objPath;
        public string fieldPath;
    }

    [System.Serializable]
    public class GlobalEventLookupTable : SerializableDictionary<SerializedFieldInfo, string>
    {
        
    }

    [System.Serializable]
    public class GlobalEventSearchTable : SerializableDictionary<string, List<SerializedFieldInfo>>
    {
        
    }

    /// <summary>
    /// The settings for global event, similar settings for local event
    /// </summary>
    [Settings( SettingsUsage.RuntimeProject, "FinTOKMAK Global Event" )]
    public sealed class GlobalEventSettings : Settings<GlobalEventSettings>
    {
        public UniversalEventConfig universalEventConfig;

        public GlobalEventLookupTable eventLookupTable;

#if UNITY_EDITOR
        [SettingsProvider]
        static SettingsProvider GetSettingsProvider() =>
            instance.GetSettingsProvider();
#endif
    }
}