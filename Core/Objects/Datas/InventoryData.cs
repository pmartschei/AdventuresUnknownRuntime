using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using AdventuresUnknownSDK.Core.Utils.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Datas/InventoryData", fileName = "InventoryData.asset")]
    public class InventoryData : IPlayerData
    {
        [SerializeField] private InventoryIdentifier m_InventoryIdentifier = null;
        [SerializeField][HideInInspector] private int m_Size = 0;
        [SerializeField][HideInInspector] private ItemStack[] m_ItemStacks = null;

        #region Properties

        #endregion

        #region Methods
        public override bool OnBeforeSerialize()
        {
            if (!ConsistencyCheck()) return false;
            m_Size = m_InventoryIdentifier.Object.Size;
            m_ItemStacks = m_InventoryIdentifier.Object.Items;
            return true;
        }
        public override bool ConsistencyCheck()
        {
            return m_InventoryIdentifier.ConsistencyCheck();
        }

        public override void Reset()
        {
            m_InventoryIdentifier.Object.Clear();
            m_Size = 0;
            m_ItemStacks = null;
        }

        public override void Load()
        {
            InventoryData inventoryData = FindScriptableObject<InventoryData>();
            if (!inventoryData) return;
            inventoryData.m_InventoryIdentifier = this.m_InventoryIdentifier;
            if (!inventoryData.m_InventoryIdentifier.ConsistencyCheck()) return;
            Inventory inventory = inventoryData.m_InventoryIdentifier.Object;
            inventory.Size = m_Size;
            inventory.Clear();
            if (m_ItemStacks == null) return;
            int i = 0;
            foreach (ItemStack itemStack in m_ItemStacks)
            {
                itemStack.ForceUpdate();
                inventory.SetItemStack(itemStack, i);
                i++;
            }
        }
        #endregion
    }
}
