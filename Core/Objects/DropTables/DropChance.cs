using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.DropTables
{
    [Serializable]
    public sealed class DropChance
    {
        [Tooltip("Drop Identifier can be DropTable Identifier or Item Identifier")]
        [SerializeField] private CoreObjectIdentifier m_DropIdentifier = new CoreObjectIdentifier(typeof(DropTable),typeof(Item));
        [Range(0,100000)]
        [SerializeField] private int m_Weight = 100;
        [Tooltip("Min Amount has only an effect if no DropTable and the item is stackable")]
        [Range(1, 1000)]
        [SerializeField] private int m_MinAmount = 1;
        [Tooltip("Max Amount has only an effect if no DropTable and the item is stackable")]
        [Range(1, 1000)]
        [SerializeField] private int m_MaxAmount = 1;

        private bool m_IsNew = true;

        #region Properties
        public string DropIdentifier => m_DropIdentifier.Identifier;
        public int MaxAmount { get => m_MaxAmount; set => m_MaxAmount = value; }
        public int MinAmount { get => m_MinAmount; set => m_MinAmount = value; }
        public int Weight { get => m_Weight; set => m_Weight = value; }
        #endregion

        #region Methods
        //Used for Editor Tool Only
        internal void UpdateIfNew()
        {
            if (m_IsNew)
            {
                DropChance dc = new DropChance();
                m_DropIdentifier = dc.m_DropIdentifier;
                m_Weight = dc.m_Weight;
                m_MinAmount = dc.m_MinAmount;
                m_MaxAmount = dc.m_MaxAmount;
                m_IsNew = false;
            }
        }
        public bool ConsistencyCheck()
        {
            return m_DropIdentifier.ConsistencyCheck();
        }
        
        //private void RecalculateDropObject()
        //{
        //    if (m_DropIdentifierDataChanged)
        //    {
        //        m_DropObject = DropTableManager.FindDropTableByIdentifier(m_DropIdentifier);
        //        if (m_DropObject == null)
        //            m_DropObject = ObjectsManager.FindObjectByIdentifier<Item>(m_DropIdentifier);
        //        m_DropIdentifierDataChanged = false;
        //    }
        //}

        public Item GetItem()
        {
            return m_DropIdentifier.Object as Item;
        }
        public DropTable GetDropTable()
        {
            return m_DropIdentifier.Object as DropTable;
        }
        #endregion
    }
}
