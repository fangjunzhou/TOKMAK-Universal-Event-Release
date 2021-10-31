using System;
using FinTOKMAK.EventSystem.Runtime;
using Hextant;
using UnityEditor;
using UnityEngine;

namespace FinTOKMAK.EventSystem.Editor
{
    public class UniversalEventDrawer : PropertyDrawer
    {
        private UniversalEventConfig _config;

        private string[] _options;
        private int _index = 0;

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                position.height = 35;
                EditorGUI.HelpBox(position, "GlobalEvent property must be used on string filed.", MessageType.Error);
                EditorGUILayout.Space(position.height - 18);
                return;
            }
            
            if (_config == null)
            {
                //_config = Settings<GlobalEventSettings>.instance.universalEventConfig;
                _config = GetEventConfig();
                
                if (_config == null)
                {
                    position.height = 35;
                    EditorGUI.HelpBox(position, "Global Event Settings missing.", MessageType.Error);
                    EditorGUILayout.Space(position.height - 18);
                    return;
                }
                _options = _config.eventNames.ToArray();
                if (_config.eventNames.Contains(property.stringValue))
                {
                    _index = _config.eventNames.IndexOf(property.stringValue);
                }
                else
                {
                    _index = 0;
                    property.stringValue = _options[_index];
                }
            }
            
            _index = EditorGUI.Popup(position, label.text, _index, _options);
            property.stringValue = _options[_index];
        }

        /// <summary>
        /// The method to get the event config for the property drawer.
        /// </summary>
        /// <returns>The event config of current event set.</returns>
        public virtual UniversalEventConfig GetEventConfig()
        {
            throw new NotImplementedException();
        }
    }
}