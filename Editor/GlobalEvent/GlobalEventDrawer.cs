using System;
using System.Collections.Generic;
using FinTOKMAK.EventSystem.Runtime;
using FinTOKMAK.EventSystem.Runtime.GlobalEvent;
using Hextant;
using UnityEditor;

namespace FinTOKMAK.EventSystem.Editor.GlobalEvent
{
    #region Public Static Field

    

    #endregion
    
    /// <summary>
    /// The event drawer for the GlobalEventSystem
    /// </summary>
    [CustomPropertyDrawer(typeof(GlobalEventAttribute))]
    public class GlobalEventDrawer: UniversalEventDrawer
    {
        public override UniversalEventConfig GetEventConfig()
        {
            return Settings<GlobalEventSettings>.instance.universalEventConfig;
        }

        public override void OnEventContentChange(SerializedProperty property, string oldEvent, string newEvent)
        {
            if (property.serializedObject.targetObject.GetType() == typeof(GlobalEventSearcherData))
                return;
            
            base.OnEventContentChange(property, oldEvent, newEvent);
            SerializedFieldInfo info = new SerializedFieldInfo()
            {
                objPath = AssetDatabase.GetAssetPath(property.serializedObject.targetObject),
                fieldPath = property.propertyPath
            };
            
            if (oldEvent == String.Empty)
            {
                // Change look up table.
                Settings<GlobalEventSettings>.instance.eventLookupTable.Add(info, newEvent);
                
                return;
            }

            // Change look up table.
            Settings<GlobalEventSettings>.instance.eventLookupTable[info] = newEvent;
        }

        public override bool HavePropertyRecord(SerializedProperty property)
        {
            SerializedFieldInfo info = new SerializedFieldInfo()
            {
                objPath = AssetDatabase.GetAssetPath(property.serializedObject.targetObject),
                fieldPath = property.propertyPath
            };

            return Settings<GlobalEventSettings>.instance.eventLookupTable.ContainsKey(info);
        }
    }
}