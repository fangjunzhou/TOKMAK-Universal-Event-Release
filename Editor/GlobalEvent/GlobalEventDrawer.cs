using FinTOKMAK.EventSystem.Runtime;
using FinTOKMAK.EventSystem.Runtime.GlobalEvent;
using Hextant;
using UnityEditor;

namespace FinTOKMAK.EventSystem.Editor.GlobalEvent
{
    /// <summary>
    /// The event drawer for the GlobalEventSystem
    /// </summary>
    [CustomPropertyDrawer(typeof(GlobalEventAttribute))]
    public class GlobalEventDrawer: UniversalEventDrawer
    {
        public override UniversalEventConfig GetEventConfig()
        {
            return Settings<GlobalEventSettings>.instance.universalEventConfig;
        }
    }
}