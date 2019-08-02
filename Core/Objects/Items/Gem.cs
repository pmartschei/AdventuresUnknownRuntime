using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Entities;
using UnityEditor;
using UnityEngine;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Log;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;
using AdventuresUnknownSDK.Core.Utils.Identifiers;

namespace AdventuresUnknownSDK.Core.Objects.Items
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Gem", fileName = "Gem.asset")]
    public class Gem : Item
    {
        [SerializeField] private Attribute[] m_Attributes = null;
        [SerializeField] private ModTypeIdentifier[] m_DisplayMods = null;

        private List<Attribute> m_ConsistentAttributes = new List<Attribute>();
        private List<ModTypeIdentifier> m_ConsistentDisplayMods = new List<ModTypeIdentifier>();

        #region Properties

        public ModTypeIdentifier[] DisplayMods { get => m_ConsistentDisplayMods.ToArray(); }
        #endregion

        #region Methods
        public override bool ConsistencyCheck()
        {
            if (!base.ConsistencyCheck()) return false;
            m_ConsistentAttributes.Clear();
            m_ConsistentDisplayMods.Clear();
            foreach (Attribute attribute in m_Attributes)
            {
                if (!attribute.ConsistencyCheck())
                {
                    GameConsole.LogWarningFormat("Skipped inconsistent Attribute: {0}", attribute.ModBaseIdentifier);
                    continue;
                }
                m_ConsistentAttributes.Add(attribute);
            }
            foreach (ModTypeIdentifier modTypeIdentifier in m_DisplayMods)
            {
                if (!modTypeIdentifier.ConsistencyCheck())
                {
                    GameConsole.LogWarningFormat("Skipped inconsistent DisplayMod: {0}", modTypeIdentifier.Identifier);
                    continue;
                }
                m_ConsistentDisplayMods.Add(modTypeIdentifier);
            }
            return true;
        }
        private ValueMod[] GetImplicits(ItemStack itemStack)
        {
            List<ValueMod> valueMods = new List<ValueMod>();

            foreach (Attribute attribute in m_ConsistentAttributes)
            {
                ValueMod valueMod = new ValueMod();
                valueMod.Value = attribute.Value(itemStack.PowerLevel);
                valueMod.Identifier = attribute.ModBaseIdentifier;
                valueMods.Add(valueMod);
            }

            return valueMods.ToArray();
        }
        public override void PowerLevelItemStackChange(ItemStack itemStack)
        {
            itemStack.ImplicitMods = GetImplicits(itemStack);
        }
        #endregion
    }
}
