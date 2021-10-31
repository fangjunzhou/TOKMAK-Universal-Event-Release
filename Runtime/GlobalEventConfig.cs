using System.Collections.Generic;
using UnityEngine;

namespace FinTOKMAK.EventSystem.Runtime
{
    /// <summary>
    /// The config file for global events
    /// </summary>
    [CreateAssetMenu(fileName = "Global Event Config", menuName = "FinTOKMAK/Global Event/Create Config", order = 0)]
    public class GlobalEventConfig : ScriptableObject
    {
        #region Public Field

        /// <summary>
        /// All the global event names
        /// </summary>
        public List<string> eventNames;

        #endregion
    }
}