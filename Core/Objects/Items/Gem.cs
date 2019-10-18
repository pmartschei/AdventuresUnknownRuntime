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
using System;
using Attribute = AdventuresUnknownSDK.Core.Objects.Mods.Attribute;

namespace AdventuresUnknownSDK.Core.Objects.Items
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Gem", fileName = "Gem.asset")]
    public class Gem : Item
    {
        [SerializeField] private string m_SupportFormatter = "";
        [SerializeField] private Attribute[] m_SupportAttributes = null;
        [SerializeField] private ModTypeIdentifier[] m_DisplaySupportMods = null;

        [NonSerialized]
        private List<Attribute> m_ConsistentSupportAttributes = new List<Attribute>();
        [NonSerialized]
        private List<ModTypeIdentifier> m_ConsistentDisplaySupportMods = new List<ModTypeIdentifier>();

        #region Properties
        public ModTypeIdentifier[] DisplaySupportMods { get => m_ConsistentDisplaySupportMods.ToArray(); }
        public string SupportFormatter { get => m_SupportFormatter; set => m_SupportFormatter = value; }
        #endregion

        #region Methods
        public override bool ConsistencyCheck()
        {
            if (!base.ConsistencyCheck()) return false;
            m_ConsistentSupportAttributes.Clear();
            m_ConsistentDisplaySupportMods.Clear();
            
            if (m_SupportAttributes != null)
            {
                foreach (Attribute attribute in m_SupportAttributes)
                {
                    if (!attribute.ConsistencyCheck())
                    {
                        GameConsole.LogWarningFormat("Skipped inconsistent Support Attribute: {0}", attribute.ModBaseIdentifier);
                        continue;
                    }
                    m_ConsistentSupportAttributes.Add(attribute);
                }
            }
            if (m_DisplaySupportMods != null)
            {
                foreach (ModTypeIdentifier modTypeIdentifier in m_DisplaySupportMods)
                {
                    if (!modTypeIdentifier.ConsistencyCheck())
                    {
                        GameConsole.LogWarningFormat("Skipped inconsistent DisplaySupportMod: {0}", modTypeIdentifier.Identifier);
                        continue;
                    }
                    m_ConsistentDisplaySupportMods.Add(modTypeIdentifier);
                }
            }
            return true;
        }

        private ValueMod[] GetExplicits(ItemStack itemStack)
        {
            List<ValueMod> valueMods = new List<ValueMod>();

            foreach (Attribute attribute in m_ConsistentSupportAttributes)
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
            itemStack.ImplicitMods = new ValueMod[0];
            itemStack.ExplicitMods = new ValueMod[0];
            itemStack.ExplicitMods = GetExplicits(itemStack);
        }
        public override bool RegisterImplicitModifiers()
        {
            return false;
        }
        public override string[] GetExplicitTexts(ItemStack itemStack)
        {
            return GetValueModsTexts(itemStack.ExplicitMods, m_SupportFormatter);
        }
        #endregion
    }
}
