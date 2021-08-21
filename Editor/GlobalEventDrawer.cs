using FinTOKMAK.GlobalEventSystem.Runtime;
using FinTOKMAK.GlobalEventSystem.Runtime.AttributeDrawers;
using Hextant;
using UnityEditor;
using UnityEngine;

namespace Package.Editor
{
    [CustomPropertyDrawer(typeof(GlobalEventAttribute))]
    public class GlobalEventDrawer : PropertyDrawer
    {
        private GlobalEventConfig _config;

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
                _config = Settings<GlobalEventSettings>.instance.globalEventConfig;
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
                }
            }
            
            _index = EditorGUI.Popup(position, label.text, _index, _options);
            property.stringValue = _options[_index];
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}