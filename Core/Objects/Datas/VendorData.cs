using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.GameModes;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items.Actions.Invoker;
using AdventuresUnknownSDK.Core.Utils.Events;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Datas/VendorData", fileName = "VendorData.asset")]
    public class VendorData : IPlayerData
    {
        [SerializeField] private DropTableIdentifier m_DropTable = null;
        [SerializeField] private InventoryIdentifier m_Inventory = null;
        [SerializeField] private int m_Seed = 0;
        [SerializeField] private List<int> m_SlotsBought = new List<int>();
        [SerializeField] private int m_PlayerLevel = 0;


        #region Properties
        public int PlayerLevel { get => m_PlayerLevel; set => m_PlayerLevel = value; }
        public List<int> SlotsBought { get => m_SlotsBought; set => m_SlotsBought = value; }

        #endregion

        #region Methods
        public override void Reset()
        {
            m_Seed = 0;
            m_SlotsBought = new List<int>();
            m_PlayerLevel = 0;
        }
        public override void Load()
        {
            VendorData vendorData = FindScriptableObject<VendorData>();
            if (!vendorData) return;
            vendorData.m_Seed = this.m_Seed;
            vendorData.m_DropTable = this.m_DropTable;
            vendorData.m_Inventory = this.m_Inventory;
            vendorData.m_PlayerLevel = this.m_PlayerLevel;
            vendorData.m_SlotsBought = this.m_SlotsBought;
            vendorData.GenerateItems(false);
        }

        public void GenerateItems(bool refill)
        {
            if (!m_Inventory.ConsistencyCheck() || !m_DropTable.ConsistencyCheck())
            {
                GameConsole.LogWarningFormat("VendorData: Either {0} or {1} is not valid", m_Inventory.Identifier, m_DropTable.Identifier);
                return;
            }
            if (refill)
            {
                m_Seed = Random.Range(0, int.MaxValue);
                m_SlotsBought.Clear();
            }
            Random.InitState(m_Seed);
            m_Inventory.Object.Clear();

            int itemCount = Random.Range(m_Inventory.Object.Size / 8, m_Inventory.Object.Size / 2) + 1;

            List<ItemStack> itemStacks = new List<ItemStack>();

            for (int i = 0; i < itemCount; i++)
            {
                ItemStack itemStack = m_DropTable.Object.Roll(m_PlayerLevel + Random.Range(0, 3), null);
                ModifierManager.ModifyItemStack(itemStack);
                if (itemStack == null) continue;
                itemStacks.Add(itemStack);
            }

            itemStacks.Sort((a, b) => { return a.Item.ItemType.TypeName.CompareTo(b.Item.ItemType.TypeName); });

            for (int i = 0; i < itemStacks.Count; i++)
            {
                if (m_SlotsBought.Contains(i)) continue;
                m_Inventory.Object.SetItemStack(itemStacks[i],i);
            }
        }

        #endregion
    }
}
