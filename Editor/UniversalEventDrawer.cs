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

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            int index = 0;
            
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
            }
            
            _options = _config.eventNames.ToArray();
            if (_config.eventNames.Contains(property.stringValue))
            {
                index = _config.eventNames.IndexOf(property.stringValue);
            }
            else
            {
                if (property.stringValue != String.Empty)
                {
                    EditorGUILayout.HelpBox($"A new event \"{property.stringValue}\" is found, do you want to add it to the event config?", MessageType.Warning);

                    EditorGUILayout.BeginHorizontal();
                    {
                        if (GUILayout.Button("Yes"))
                        {
                            _config.eventNames.Add(property.stringValue);
                            EditorUtility.SetDirty(_config);
                            index = _config.eventNames.IndexOf(property.stringValue);
                        }

                        if (GUILayout.Button("No"))
                        {
                            index = 0;
                            property.stringValue = _options[index];
                        }   
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    return;
                }
                else
                {
                    index = 0;
                    property.stringValue = _options[index];
                }
            }
            
            EditorGUI.BeginChangeCheck();
            index = EditorGUI.Popup(position, label.text, index, _options);
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log("Drop down changed.");
                property.stringValue = _options[index];
            }
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