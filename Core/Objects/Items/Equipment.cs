using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;

namespace AdventuresUnknownSDK.Core.Objects.Items
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Equipment", fileName = "Equipment.asset")]
    public class Equipment : Item
    {
        [SerializeField] private bool m_IsUnique = false;
        [SerializeField] private ModIdentifier[] m_ImplicitMods = null;
        [SerializeField] private ModIdentifier[] m_ExplicitMods = null;

        [NonSerialized]
        protected bool[] m_IsImplicitValid;
        [NonSerialized]
        protected bool[] m_IsExplicitValid;

        #region Methods
        public override bool ConsistencyCheck()
        {
            if (!base.ConsistencyCheck()) return false;
            int implicitLength = m_ImplicitMods == null ? 0 : m_ImplicitMods.Length;
            int explicitLength = m_ExplicitMods == null ? 0 : m_ExplicitMods.Length;
            m_IsImplicitValid = new bool[implicitLength];
            for(int i = 0; i < implicitLength; i++)
            {
                m_IsImplicitValid[i] = m_ImplicitMods[i].ConsistencyCheck();
            }
            m_IsExplicitValid = new bool[explicitLength];
            for (int i = 0; i < explicitLength; i++)
            {
                m_IsExplicitValid[i] = m_ExplicitMods[i].ConsistencyCheck();
            }
            return true;
        }
        public override ItemStack CreateItem(int amount)
        {
            ItemStack itemStack = base.CreateItem(amount);

            List<ValueMod> implicitsValueMods = new List<ValueMod>();
            List<ValueMod> explicitsValueMods = new List<ValueMod>();
            for (int i = 0; i < m_ImplicitMods.Length; i++)
            {
                if (!m_IsImplicitValid[i]) continue;
                implicitsValueMods.Add(m_ImplicitMods[i].Object.Roll());
            }
            for (int i = 0; i < m_ExplicitMods.Length; i++)
            {
                if (!m_IsExplicitValid[i]) continue;
                explicitsValueMods.Add(m_ExplicitMods[i].Object.Roll());
            }
            itemStack.ImplicitMods = implicitsValueMods.ToArray();
            itemStack.ExplicitMods = explicitsValueMods.ToArray();
            return itemStack;
        }
        public override bool IsModifiable()
        {
            return !m_IsUnique;
        }
        #endregion
    }
}
