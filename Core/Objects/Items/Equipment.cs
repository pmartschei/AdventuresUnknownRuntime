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

namespace AdventuresUnknownSDK.Core.Objects.Items
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Equipment", fileName = "Equipment.asset")]
    public class Equipment : Item
    {
        [SerializeField] private bool m_IsUnique = false;
        [SerializeField] private ModIdentifier[] m_ImplicitMods = null;
        [SerializeField] private bool m_CanRerollProperties = true;
        [SerializeField] private bool m_CanHaveExplicits = false;

        [NonSerialized]
        protected bool[] m_IsImplicitValid;
        
        #region Methods
        public override bool ConsistencyCheck()
        {
            if (!base.ConsistencyCheck()) return false;
            m_IsImplicitValid = new bool[m_ImplicitMods.Length];
            for(int i = 0; i < m_ImplicitMods.Length; i++)
            {
                m_IsImplicitValid[i] = m_ImplicitMods[i].ConsistencyCheck();
            }
            return true;
        }
        #endregion
    }
}
