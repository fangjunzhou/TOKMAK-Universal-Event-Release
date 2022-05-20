using System;
using System.Collections.Generic;
using System.Linq;
using FinTOKMAK.EventSystem.Runtime.GlobalEvent;
using Hextant;
using UnityEditor;
using UnityEngine;

namespace FinTOKMAK.EventSystem.Editor.GlobalEvent
{
    public class GlobalEventSearcher : EditorWindow
    {
        #region Private Field

        private GlobalEventSearcherData _data;
        
        private SerializedProperty _searchEvent;

        private List<SerializedFieldInfo> _result = new List<SerializedFieldInfo>();

        #endregion
        
        [MenuItem("FinTOKMAK/Universal Event/Global Event Searcher")]
        private static void ShowWindow()
        {
            var window = GetWindow<GlobalEventSearcher>();
            window.titleContent = new GUIContent("Global Event Searcher");
            window.Show();
        }

        private void OnEnable()
        {
            _data = ScriptableObject.CreateInstance<GlobalEventSearcherData>();
            _searchEvent = new SerializedObject(_data).FindProperty("searchTarget");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal("box");
            {
                EditorGUILayout.PropertyField(_searchEvent, GUILayout.ExpandWidth(true));
                
                GUILayout.Space(15);

                if (GUILayout.Button("Search", GUILayout.Width(50)))
                {
                    _result = Settings<GlobalEventSettings>.instance.eventLookupTable.Keys
                        .Where((info => Settings<GlobalEventSettings>.instance.eventLookupTable[info] ==
                                        _searchEvent.stringValue))
                        .ToList();

                    if (_result.Count == 0)
                        EditorUtility.DisplayDialog("Result Not Found",
                            $"There is no event named {_searchEvent.stringValue} in the database.", "OK");
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.LabelField("Search Result", EditorStyles.boldLabel);
                foreach (SerializedFieldInfo info in _result)
                {
                    EditorGUILayout.BeginVertical("box");
                    {
                        EditorGUILayout.TextField("Object: ", info.objPath);
                        
                        EditorGUILayout.TextField("Field: ", info.fieldPath);
                    }
                    EditorGUILayout.EndVertical();
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}