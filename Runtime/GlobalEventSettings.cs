using Hextant;
#if UNITY_EDITOR
using Hextant.Editor;
using UnityEditor;
#endif

namespace FinTOKMAK.EventSystem.Runtime
{
    [Settings( SettingsUsage.RuntimeProject, "FinTOKMAK Global Event" )]
    public sealed class GlobalEventSettings : Settings<GlobalEventSettings>
    {
        public GlobalEventConfig globalEventConfig;
        
#if UNITY_EDITOR
        [SettingsProvider]
        static SettingsProvider GetSettingsProvider() =>
            instance.GetSettingsProvider();
#endif
    }
}