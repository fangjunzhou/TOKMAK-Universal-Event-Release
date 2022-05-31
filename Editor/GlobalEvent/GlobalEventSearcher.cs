using System;
using System.Collections.Generic;
using System.Linq;
using FinTOKMAK.EventSystem.Runtime.GlobalEvent;
using Hextant;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FinTOKMAK.EventSystem.Editor.GlobalEvent
{
    public class GlobalEventSearcher : EditorWindow
    {
        private class SearchResult
        {
            public SerializedFieldInfo info;
            public Object obj;
        }
        
        #region Private Field

        private GlobalEventSearcherData _data;
        
        private SerializedProperty _searchEvent;

        private List<SearchResult> _result = new List<SearchResult>();

        private Vector2 _resultSearchScrollView;

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
                EditorGUILayout.PropertyField(_searchEvent);
                
                GUILayout.Space(10);

                if (GUILayout.Button("Search", GUILayout.Width(50)))
                {
                    _result = Settings<GlobalEventSettings>.instance.eventLookupTable.Keys
                        .Where((info => Settings<GlobalEventSettings>.instance.eventLookupTable[info] ==
                                        _searchEvent.stringValue))
                        .Select((info => new SearchResult()
                        {
                            info = info,
                            obj = AssetDatabase.LoadAssetAtPath<Object>(info.objPath)
                        }))
                        .ToList();

                    if (_result.Count == 0)
                        EditorUtility.DisplayDialog("Result Not Found",
                            $"There is no event named \"{_searchEvent.stringValue}\" in the database.", "OK");
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.BeginVertical("box");
            {
                EditorGUILayout.LabelField("Search Result", EditorStyles.boldLabel);
                _resultSearchScrollView = EditorGUILayout.BeginScrollView(_resultSearchScrollView);
                {
                    foreach (SearchResult result in _result)
                    {
                        EditorGUILayout.BeginVertical("box");
                        {
                            EditorGUILayout.ObjectField("Object: ", result.obj, typeof(Object));

                            EditorGUILayout.TextField("Object Path: ", result.info.objPath);
                        
                            EditorGUILayout.TextField("Field: ", result.info.fieldPath);
                        }
                        EditorGUILayout.EndVertical();
                    } 
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndVertical();
        }
    }
}