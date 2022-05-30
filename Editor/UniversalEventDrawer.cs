using System;
using System.Linq;
using FinTOKMAK.EventSystem.Runtime;
using Hextant;
using UnityEditor;
using UnityEngine;

namespace FinTOKMAK.EventSystem.Editor
{
    public class UniversalEventDrawer : PropertyDrawer
    {
        private bool _init = false;
        
        private UniversalEventConfig _config;

        private string[] _options;

        private bool _checkTable = false;

        protected virtual void Init(Rect position, SerializedProperty property,
            GUIContent label)
        {
        }

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            if (!_init)
            {
                Init(position, property, label);
                _init = true;
            }
            
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
                            OnEventContentChange(property, string.Empty, _options[index]);
                            
                        }

                        if (GUILayout.Button("No"))
                        {
                            index = 0;
                            property.stringValue = _options[index];
                            OnEventContentChange(property, string.Empty, _options[index]);
                        }   
                    }
                    EditorGUILayout.EndHorizontal();
                    
                    return;
                }
                else
                {
                    index = 0;
                    property.stringValue = _options[index];
                    OnEventContentChange(property, string.Empty, _options[index]);
                }
            }

            if (!_checkTable)
            {
                if (!HavePropertyRecord(property))
                {
                    OnEventContentChange(property, string.Empty, _options[index]);
                }
                _checkTable = true;
            }

            position.width = position.width - 110;
            
            EditorGUI.BeginChangeCheck();
            int oldIndex = index;
            index = EditorGUI.Popup(position, label.text, index, _options);
            if (EditorGUI.EndChangeCheck())
            {
                Debug.Log($"Event changed from {_options[oldIndex]} to {_options[index]}.");
                property.stringValue = _options[index];
                OnEventContentChange(property, _options[oldIndex], _options[index]);
                
            }
        
            position.x = position.x + position.width + 5;
            position.width = 50;
            
            if (GUI.Button(position, "Copy"))
            {
                EditorGUIUtility.systemCopyBuffer = _options[index];
            }
            
            position.x = position.x + position.width + 5;
            position.width = 50;
            
            if (GUI.Button(position, "Paste"))
            {
                string content = EditorGUIUtility.systemCopyBuffer;
                if (_options.Contains(content))
                {
                    oldIndex = index;
                    index = _options.ToList().IndexOf(content);
                    property.stringValue = _options[index];
                    OnEventContentChange(property, _options[oldIndex], _options[index]);
                }
                else
                {
                    EditorUtility.DisplayDialog("Event Not Found",
                        $"There is no event named \"{content}\" in the database.", "OK");
                }
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

        public virtual void OnEventContentChange(SerializedProperty property, string oldEvent, string newEvent)
        {
            
        }

        public virtual bool HavePropertyRecord(SerializedProperty property)
        {
            return false;
        }
    }
}