using System;
using System.Collections.Generic;
using System.Linq;
using FinTOKMAK.EventSystem.Runtime;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace FinTOKMAK.EventSystem.Editor
{
    /// <summary>
    /// The interface for PathDirectory and PathEvent
    /// </summary>
    public interface IPathElement
    {
        bool isDirectory { get; }
        string displayName { get; }
        List<IPathElement> GetChildren();

        /// <summary>
        /// Call this method to draw the UI of the PathElement
        /// </summary>
        /// <param name="rect">the rect transform of the element</param>
        /// <param name="index">the index to draw</param>
        /// <param name="active">is the element active</param>
        /// <param name="focused">is the element focused</param>
        void DrawPathElement(Rect rect, int index, bool active, bool focused);

        /// <summary>
        /// Get the height of editor element
        /// </summary>
        float editorHeight { get; }
    }
    
    /// <summary>
    /// The directory for skill names
    /// </summary>
    public class PathDirectory : IPathElement
    {
        /// <summary>
        /// The parent directory
        /// </summary>
        public PathDirectory parentDirectory;
        
        /// <summary>
        /// The directory name
        /// </summary>
        public string name;

        /// <summary>
        /// The path of the directory
        /// </summary>
        public string path;

        /// <summary>
        /// The path of its parent directory
        /// </summary>
        public string parentPath;

        /// <summary>
        /// All the sub directories in the current directory
        /// </summary>
        public Dictionary<string, PathDirectory> subDirectories;

        /// <summary>
        /// All the events in current directory
        /// </summary>
        public Dictionary<string, PathEvent> events;

        #region Editor Variables
        
        /// <summary>
        /// All the children in the current directory
        /// </summary>
        private List<IPathElement> children;

        /// <summary>
        /// The reorderable list
        /// </summary>
        private ReorderableList reorderableList;

        /// <summary>
        /// The height of editor
        /// </summary>
        private float _editorHeight = 0;

        /// <summary>
        /// if the current IPathElement is a directory 
        /// </summary>
        private bool _addDirectory = false;

        /// <summary>
        /// If the directory is expanded
        /// </summary>
        private bool _expand = false;

        #endregion

        /// <summary>
        /// The constructor of a root EventDirectory
        /// </summary>
        public PathDirectory()
        {
            subDirectories = new Dictionary<string, PathDirectory>();
            events = new Dictionary<string, PathEvent>();
            this.name = "root";
            this.path = "root";
            parentPath = null;

            OnEnable();
        }

        /// <summary>
        /// The constructor of an non-root EventDirectory
        /// </summary>
        /// <param name="name">the name of current directory</param>
        /// <param name="parentPath">the path of its parent directory</param>
        public PathDirectory(string name, PathDirectory parentDirectory)
        {
            this.parentDirectory = parentDirectory;
            subDirectories = new Dictionary<string, PathDirectory>();
            events = new Dictionary<string, PathEvent>();
            this.name = name;
            this.path = parentDirectory.path + "/" + name;
            this.parentPath = parentDirectory.path;
            
            OnEnable();
        }

        /// <summary>
        /// Call this method to add a new event to the current directory
        /// </summary>
        /// <param name="name">the name of the event</param>
        public void AddEvent(string name)
        {
            PathEvent pathEvent = new PathEvent(name, this);
            events.Add(name, pathEvent);
            
            UpdateChildList();
        }

        /// <summary>
        /// Call this method to remove a event from current directory
        /// </summary>
        /// <param name="name">the name of the event</param>
        public void RemoveEvent(string name)
        {
            events.Remove(name);
        }

        /// <summary>
        /// Call this method to add a sub directory to the current directory
        /// </summary>
        /// <param name="name">the name of the directory</param>
        public void AddDirectory(string name)
        {
            PathDirectory directory = new PathDirectory(name, this);
            subDirectories.Add(name, directory);
            
            UpdateChildList();
        }

        /// <summary>
        /// Call this method to remove a directory from current directory
        /// </summary>
        /// <param name="name">the name of the directory</param>
        public void RemoveDirectory(string name)
        {
            subDirectories.Remove(name);
        }

        /// <summary>
        /// Call this method to rename the directory
        /// </summary>
        /// <param name="newName"></param>
        public void Rename(string newName)
        {
            // Cannot rename root directory
            if (parentPath == null)
            {
                throw new InvalidOperationException("Cannot rename root directory.");
            }
            
            // remove the old key-value pair in the parent directory
            parentDirectory.subDirectories.Remove(this.name);
            
            // Update the current name and path
            this.name = newName;
            this.path = parentPath + "/" + name;
            
            // change the key in parent directory
            parentDirectory.subDirectories.Add(this.name, this);
            
            // Update the parent path of all the events
            foreach (PathEvent pathEvent in events.Values)
            {
                pathEvent.UpdateParentPath(this.path);
            }
            // Update the parent path of all the sub directories
            foreach (PathDirectory directory in subDirectories.Values)
            {
                directory.UpdateParentPath(this.path);
            }
        }

        /// <summary>
        /// Update the parent path of current event
        /// </summary>
        /// <param name="newParentPath">the new parent path</param>
        public void UpdateParentPath(string newParentPath)
        {
            // Cannot update parent path for root directory
            if (parentPath == null)
            {
                throw new InvalidOperationException("Cannot update parent path root directory.");
            }
            
            this.parentPath = newParentPath;
            this.path = parentPath + "/" + name;
            
            // Update the parent path of all the events
            foreach (PathEvent pathEvent in events.Values)
            {
                pathEvent.UpdateParentPath(this.path);
            }
            // Update the parent path of all the sub directories
            foreach (PathDirectory directory in subDirectories.Values)
            {
                directory.UpdateParentPath(this.path);
            }
        }

        /// <summary>
        /// Get all the event paths
        /// </summary>
        /// <returns>the path of all the events in the directory</returns>
        public string[] GetAllEvents()
        {
            string[] res = new string[0];
            // get all the events in sub directories
            foreach (PathDirectory directory in subDirectories.Values)
            {
                res = MergeArray(res, directory.GetAllEvents());
            }
            // get the events in current directory
            PathEvent[] pathEvents = events.Values.ToArray();
            string[] currentEvents = new string[pathEvents.Length];
            for (int i = 0; i < pathEvents.Length; i++)
            {
                currentEvents[i] = pathEvents[i].path;
            }

            res = MergeArray(res, currentEvents);

            return res;
        }

        /// <summary>
        /// Call this method to merge two array
        /// </summary>
        /// <param name="array1">array 1</param>
        /// <param name="array2">array 2</param>
        /// <typeparam name="T">the generic type of the array elements</typeparam>
        /// <returns>the merged array</returns>
        private T[] MergeArray<T>(T[] array1, T[] array2)
        {
            T[] res = new T[array1.Length + array2.Length];
            
            for (int i = 0; i < array1.Length; i++)
            {
                res[i] = array1[i];
            }

            for (int i = 0; i < array2.Length; i++)
            {
                res[array1.Length + i] = array2[i];
            }

            return res;
        }

        public bool isDirectory
        {
            get
            {
                return true;
            }
        }

        public string displayName
        {
            get
            {
                return name;
            }
        }

        public List<IPathElement> GetChildren()
        {
            IPathElement[] directories = subDirectories.Values.ToArray();
            IPathElement[] events = this.events.Values.ToArray();
            
            return MergeArray(directories, events).ToList();
        }

        public void DrawPathElement(Rect rect, int index, bool active, bool focused)
        {
            _editorHeight = 0;

            if (_expand)
            {
                reorderableList.DoList(rect);
                
                _editorHeight += reorderableList.GetHeight();
                rect.y += reorderableList.GetHeight();
                rect.height = 18;
            
                // Change the add mode
                if (_addDirectory)
                {
                    if (GUI.Button(rect, "Current: Add Directory"))
                    {
                        _addDirectory = false;
                    }
                }
                else
                {
                    if (GUI.Button(rect, "Current: Add Event"))
                    {
                        _addDirectory = true;
                    }
                }

                _editorHeight += 18;
                rect.y += 18;
                rect.height = 18;
                
                if (GUI.Button(rect, "Fold↑"))
                {
                    _expand = false;
                    DrawPathElement(rect, index, active, focused);
                }
                _editorHeight += 18;
            }
            else
            {
                rect.height = 18;
                EditorGUI.LabelField(rect, name, EditorStyles.boldLabel);
                _editorHeight += 18;
                rect.y += 18;
                rect.height = 18;

                if (GUI.Button(rect, "Expand↓"))
                {
                    _expand = true;
                    DrawPathElement(rect, index, active, focused);
                }
                _editorHeight += 18;
            }
        }

        private void OnEnable()
        {
            children = GetChildren();
            reorderableList = new ReorderableList(children, typeof(IPathElement));
            // the callback to draw the header
            reorderableList.drawHeaderCallback += rect1 =>
            {
                string newName = EditorGUI.TextField(rect1, name);
                if (newName != name)
                {
                    Rename(newName);
                }
            };
            // the callback to draw each element
            reorderableList.drawElementCallback += (rect, index, active, focused) =>
            {
                children[index].DrawPathElement(rect, index, active, focused);
            };
            // the callback to get the height of each element
            reorderableList.elementHeightCallback += index =>
            {
                //Debug.Log(rootChidren[index].editorHeight);
                return children[index].editorHeight;
            };
            // add a new event or directory
            reorderableList.onAddCallback += list =>
            {
                if (!_addDirectory)
                {
                    int index = 0;
                    string eventName = "NEW_EVENT_" + index.ToString();
                    while (this.events.ContainsKey(eventName))
                    {
                        index++;
                        eventName = "NEW_EVENT_" + index.ToString();
                    }

                    this.AddEvent(eventName);
                }
                else
                {
                    int index = 0;
                    string directoryName = "NEW_DIRECTORY_" + index.ToString();
                    while (this.subDirectories.ContainsKey(directoryName))
                    {
                        index++;
                        directoryName = "NEW_DIRECTORY_" + index.ToString();
                    }

                    this.AddDirectory(directoryName);
                }
                
                UpdateChildList();
                reorderableList.DoLayoutList();
            };
            reorderableList.onRemoveCallback += list =>
            {
                IPathElement selected = children[list.index];
                if (selected.isDirectory)
                {
                    ((PathDirectory)selected).parentDirectory.RemoveDirectory(((PathDirectory)selected).name);
                }
                else
                {
                    ((PathEvent)selected).parentDirectory.RemoveEvent(((PathEvent)selected).name);
                }
                children.Clear();
                foreach (IPathElement child in this.GetChildren())
                {
                    children.Add(child);
                }
            };
        }

        /// <summary>
        /// Call this method to update the child list
        /// </summary>
        private void UpdateChildList()
        {
            children.Clear();
            foreach (IPathElement child in this.GetChildren())
            {
                children.Add(child);
            }
        }

        public float editorHeight
        {
            get
            {
                return _editorHeight;
            }
        }
    }

    /// <summary>
    /// The event with path info using in the directory
    /// </summary>
    public class PathEvent : IPathElement
    {
        /// <summary>
        /// The parent directory
        /// </summary>
        public PathDirectory parentDirectory;
        /// <summary>
        /// The real name of the event
        /// </summary>
        public string name;
        /// <summary>
        /// The full path of the event
        /// </summary>
        public string path;
        /// <summary>
        /// The path of its parent directory
        /// </summary>
        public string parentPath;

        /// <summary>
        /// The editor height
        /// </summary>
        private float _editorHeight = 0;

        /// <summary>
        /// The constructor of PathEvent
        /// </summary>
        /// <param name="name">the name of the event</param>
        /// <param name="parentPath">the path of it's parent directory</param>
        public PathEvent(string name, PathDirectory parentDirectory)
        {
            this.parentDirectory = parentDirectory;
            this.name = name;
            this.path = parentDirectory.path + "/" + name;
            this.parentPath = parentDirectory.path;
        }

        /// <summary>
        /// Call this method to rename the PathEvent
        /// </summary>
        /// <param name="newName">the new name of the event</param>
        public void Rename(string newName)
        {
            // remove the old key-value pair in the parent directory
            parentDirectory.events.Remove(this.name);
            
            this.name = newName;
            this.path = parentPath + "/" + name;
            
            // change the key in parent directory
            parentDirectory.events.Add(this.name, this);
        }

        /// <summary>
        /// Update the parent path of current event
        /// </summary>
        /// <param name="newParentPath">the new parent path</param>
        public void UpdateParentPath(string newParentPath)
        {
            this.parentPath = newParentPath;
            this.path = parentPath + "/" + name;
        }

        public bool isDirectory
        {
            get
            {
                return false;
            }
        }

        public string displayName
        {
            get
            {
                return name;
            }
        }

        public List<IPathElement> GetChildren()
        {
            return null;
        }

        public void DrawPathElement(Rect rect, int index, bool active, bool focused)
        {
            _editorHeight = 0;
            rect.height = 18;
            string newName = EditorGUI.TextField(rect, displayName);
            if (newName != name)
            {
                Rename(newName);
            }

            _editorHeight += rect.height;
        }

        public float editorHeight
        {
            get
            {
                return _editorHeight;
            }
        }
    }
    
    [CustomEditor(typeof(UniversalEventConfig))]
    public class UniversalEventConfigEditor : UnityEditor.Editor
    {
        #region Private Field

        private PathDirectory _root;

        /// <summary>
        /// If the new element added is a directory
        /// </summary>
        private bool _addDirectory = false;

        private List<IPathElement> rootChidren;
        private ReorderableList rootList;

        #endregion

        private void OnEnable()
        {
            _root = new PathDirectory();
            
            ReadFromConfig();
            
            rootChidren = _root.GetChildren();
            
            rootList = new ReorderableList(rootChidren, typeof(IPathElement), true, true, true, true);
            // Draw the root's header as label root
            rootList.drawHeaderCallback += rect => { EditorGUI.LabelField(rect, "Root"); };
            // the callback to draw each element
            rootList.drawElementCallback += (rect, index, active, focused) =>
            {
                rootChidren[index].DrawPathElement(rect, index, active, focused);
            };
            // the callback to get the height of each element
            rootList.elementHeightCallback += index =>
            {
                //Debug.Log(rootChidren[index].editorHeight);
                return rootChidren[index].editorHeight;
            };
            // add a new event or event directory
            rootList.onAddCallback += list =>
            {
                if (!_addDirectory)
                {
                    int index = 0;
                    string eventName = "NEW_EVENT_" + index.ToString();
                    while (_root.events.ContainsKey(eventName))
                    {
                        index++;
                        eventName = "NEW_EVENT_" + index.ToString();
                    }

                    _root.AddEvent(eventName);
                }
                else
                {
                    int index = 0;
                    string directoryName = "NEW_DIRECTORY_" + index.ToString();
                    while (_root.subDirectories.ContainsKey(directoryName))
                    {
                        index++;
                        directoryName = "NEW_DIRECTORY_" + index.ToString();
                    }

                    _root.AddDirectory(directoryName);
                }
                rootChidren.Clear();
                foreach (IPathElement child in _root.GetChildren())
                {
                    rootChidren.Add(child);
                }
                rootList.DoLayoutList();
            };
            rootList.onRemoveCallback += list =>
            {
                IPathElement selected = rootChidren[list.index];
                if (selected.isDirectory)
                {
                    if (!EditorUtility.DisplayDialog("Warning!", "You are removing a directory, are you sure to do that?",
                        "Yes", "Cancel"))
                    {
                        return;
                    }
                    ((PathDirectory)selected).parentDirectory.RemoveDirectory(((PathDirectory)selected).name);
                }
                else
                {
                    ((PathEvent)selected).parentDirectory.RemoveEvent(((PathEvent)selected).name);
                }
                rootChidren.Clear();
                foreach (IPathElement child in _root.GetChildren())
                {
                    rootChidren.Add(child);
                }
            };
        }

        public override void OnInspectorGUI()
        {
            rootList.DoLayoutList();

            // Change the add mode
            if (_addDirectory)
            {
                if (GUILayout.Button("Current: Add Directory"))
                {
                    _addDirectory = false;
                }
            }
            else
            {
                if (GUILayout.Button("Current: Add Event"))
                {
                    _addDirectory = true;
                }
            }
            
            EditorGUILayout.Space(10);

            if (GUILayout.Button("Save"))
            {
                if (EditorUtility.DisplayDialog("Warning!",
                    "This will override the old data in the config, are you sure to do that?", "Yes", "Cancel"))
                {
                    SaveAllPath();
                }
            }
        }

        /// <summary>
        /// Call this method to save all the path
        /// </summary>
        private void SaveAllPath()
        {
            UniversalEventConfig config = (UniversalEventConfig) serializedObject.targetObject;
            config.eventNames = _root.GetAllEvents().ToList();
            EditorUtility.SetDirty(config);
            AssetDatabase.SaveAssetIfDirty(config);
        }

        /// <summary>
        /// Call this method to read from config
        /// </summary>
        private void ReadFromConfig()
        {
            UniversalEventConfig config = (UniversalEventConfig) serializedObject.targetObject;

            if (config.eventNames == null)
            {
                config.eventNames = new List<string>();
            }
            
            foreach (string path in config.eventNames)
            {
                PathDirectory workingDirectory = _root;
                string[] pathArray = path.Split('/');
                for (int i = 1; i < pathArray.Length; i++)
                {
                    // If go to the final path, add the event to the working directory
                    if (i == pathArray.Length - 1)
                    {
                        workingDirectory.AddEvent(pathArray[i]);
                        break;
                    }
                    // check if working directory has specific sub directory
                    if (!workingDirectory.subDirectories.ContainsKey(pathArray[i]))
                    {
                        workingDirectory.AddDirectory(pathArray[i]);
                    }

                    // update the working directory(go into the new directory)
                    workingDirectory = workingDirectory.subDirectories[pathArray[i]];
                }
            }
        }
    }
}