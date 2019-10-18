using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Items
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/DisplayMods", fileName = "DisplayMods.asset")]
    public class DisplayMods : ScriptableObject
    {
        [SerializeField] private ModTypeIdentifier[] m_Mods = null;

        private List<ModTypeIdentifier> m_ConsistentDisplayMods = new List<ModTypeIdentifier>();
        public ModTypeIdentifier[] Mods { get => m_ConsistentDisplayMods.ToArray(); }

        public void ConsistencyCheck()
        {
            m_ConsistentDisplayMods.Clear();
            foreach (ModTypeIdentifier modTypeIdentifier in m_Mods)
            {
                if (!modTypeIdentifier.ConsistencyCheck())
                {
                    GameConsole.LogWarningFormat("Skipped inconsistent DisplayMod: {0}", modTypeIdentifier.Identifier);
                    continue;
                }
                m_ConsistentDisplayMods.Add(modTypeIdentifier);
            }
        }
    }
}
