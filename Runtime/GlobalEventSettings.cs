using FinTOKMAK.GlobalEventSystem.Runtime;
using Hextant;
using Hextant.Editor;
using UnityEditor;
using UnityEngine;

namespace Package.Editor
{
    [Settings( SettingsUsage.EditorProject, "FinTOKMAK Global Event" )]
    public sealed class GlobalEventSettings : Settings<GlobalEventSettings>
    {
        public GlobalEventConfig globalEventConfig;
        
        [SettingsProvider]
        static SettingsProvider GetSettingsProvider() =>
            instance.GetSettingsProvider();
    }
}