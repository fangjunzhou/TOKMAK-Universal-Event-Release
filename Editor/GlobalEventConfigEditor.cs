using System;
using System.Collections.Generic;
using System.Linq;
using FinTOKMAK.GlobalEventSystem.Runtime;
using UnityEditor;
using UnityEditorInternal;

namespace Package.Editor
{
    /// <summary>
    /// The interface for PathDirectory and PathEvent
    /// </summary>
    public interface IPathElement
    {
        string displayName { get; set; }
        IPathElement[] containElements();
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

        /// <summary>
        /// The constructor of a root EventDirectory
        /// </summary>
        public PathDirectory()
        {
            subDirectories = new Dictionary<string, PathDirectory>();
            events = new Dictionary<string, PathEvent>();
            this.name = "root";
            this.path = "";
            parentPath = null;
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
        }

        /// <summary>
        /// Call this method to add a new event to the current directory
        /// </summary>
        /// <param name="name">the name of the event</param>
        public void AddEvent(string name)
        {
            PathEvent pathEvent = new PathEvent(name, this);
            events.Add(name, pathEvent);
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

        public string displayName
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        public IPathElement[] containElements()
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// The event with path info using in the directory
    /// </summary>
    public class PathEvent
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
    }
    
    [CustomEditor(typeof(GlobalEventConfig))]
    public class GlobalEventConfigEditor : UnityEditor.Editor
    {
        #region Private Field

        private PathDirectory _root;

        #endregion

        private void OnEnable()
        {
            _root = new PathDirectory();
        }

        public override void OnInspectorGUI()
        {
            
        }
    }
}