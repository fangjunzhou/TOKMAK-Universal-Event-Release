using FinTOKMAK.EventSystem.Runtime.GlobalEvent;
using UnityEngine;

namespace FinTOKMAK.EventSystem.Editor.GlobalEvent
{
    public class GlobalEventSearcherData : ScriptableObject
    {
        [GlobalEvent]
        public string searchTarget;
    }
}