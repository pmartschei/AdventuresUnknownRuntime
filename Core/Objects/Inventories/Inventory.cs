using AdventuresUnknownRuntime.Core.Utils.Identifiers;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Utils.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Inventories
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Inventory/Inventory", fileName = "Inventory.asset")]
    public class Inventory : CoreObject, IActiveStat
    {
        [Range(0, 100)]
        [SerializeField] private int m_Size = 10;
        [SerializeField] private List<ItemStack> m_ItemStacks = new List<ItemStack>();

        [NonSerialized]
        private IntEvent m_OnSlotUpdate = new IntEvent();
        [NonSerialized]
        private bool m_StatsChanged = false;

        #region Properties
        public int Size
        {
            get => m_Size;
            set
            {
                if (m_Size != value)
                {
                    m_Size = value;
                    ResizeList();
                }
            }
        }

        protected int NextEmptySlot
        {
            get; private set;
        }
        public ItemStack[] Items
        {
            get { return m_ItemStacks.ToArray(); }
        }
        public IntEvent OnSlotUpdateEvent
        {
            get { return m_OnSlotUpdate; }
        }

        public bool StatsChanged
        {
            get
            {
                if (m_StatsChanged) return true;
                foreach (ItemStack itemStack in m_ItemStacks)
                {
                    if (itemStack.StatsChanged) return true;
                }
                return false;
            }
            set
            {
                m_StatsChanged = value;
                foreach (ItemStack itemStack in m_ItemStacks)
                {
                    itemStack.StatsChanged = value;
                }
            }
        }
        #endregion

        #region Methods

        public void Initialize(Entity activeStat)
        {
            foreach (ItemStack itemStack in m_ItemStacks)
            {
                if (itemStack.IsEmpty) continue;
                itemStack.Initialize(activeStat);
            }
        }
        public override void Initialize()
        {
            NextEmptySlot = 0;
            ResizeList();
            for (int i = 0; i < m_ItemStacks.Count; i++)
            {
                if (m_ItemStacks[i].Amount == 0)
                {
                    RemoveItemStack(i);
                    continue;
                }
                m_ItemStacks[i].ForceUpdate();
            }
        }
        public bool AddItemStack(ItemStack itemStack)
        {
            if (itemStack == null) return true;
            if (itemStack.Amount <= 0) return true;
            int slotToInsert = -1;
            if (itemStack.Item != null && itemStack.Item.IsStackable)
            {
                slotToInsert = FindItemSlotIndexByIdentifier(itemStack.Item.Identifier);
            }
            if (slotToInsert == -1)
                slotToInsert = NextEmptySlot;
            if (slotToInsert >= m_Size || slotToInsert == -1) return false;

            ItemStack slotItemStack = m_ItemStacks[slotToInsert];

            StatsChanged = true;

            if (slotItemStack.Item == null)
            {
                m_ItemStacks[slotToInsert] = itemStack;
                //slotItemStack.ItemIdentifier = itemStack.ItemIdentifier;
                //slotItemStack.Amount = itemStack.Amount;
                //slotItemStack.PowerLevel = itemStack.PowerLevel;
                //slotItemStack.ImplicitMods = itemStack.ImplicitMods;
                //slotItemStack.ExplicitMods = itemStack.ExplicitMods;
                UpdateSlot(slotToInsert);
                return true;
            }
            else
            {
                int remainder = slotItemStack.AddAmount(itemStack.Amount);
                itemStack.Amount = remainder;
                UpdateSlot(slotToInsert);
            }
            FindNextEmptySlot();
            return AddItemStack(itemStack);
        }

        public void SetItemStack(ItemStack itemStack, int index)
        {
            if (index < 0 || index >= m_ItemStacks.Count) return;
            m_ItemStacks[index] = itemStack;
            if (itemStack.IsEmpty && index < NextEmptySlot)
            {
                NextEmptySlot = index;
            }
            else if (index == NextEmptySlot)
            {
                FindNextEmptySlot();
            }
            UpdateSlot(index);
        }

        public void RemoveItemStack(int index)
        {
            if (index < 0 || index >= m_ItemStacks.Count) return;
            m_ItemStacks[index].ItemIdentifier = "";
            //m_ItemStacks[index].Amount = 0;
            m_ItemStacks[index].PowerLevel = 0;
            m_ItemStacks[index].ImplicitMods = new ItemStack.ValueMod[0];
            m_ItemStacks[index].ExplicitMods = new ItemStack.ValueMod[0];
            UpdateSlot(index);
            if (index < NextEmptySlot) NextEmptySlot = index;
            StatsChanged = true;
        }

        public bool SwitchItemStacks(int originIndex, Inventory destinationInventory, int destinationIndex)
        {
            if (destinationInventory == null) return false;
            if (originIndex >= m_ItemStacks.Count ||
                destinationIndex >= destinationInventory.m_ItemStacks.Count || originIndex < 0 || destinationIndex < 0) return false;
            ItemStack originItemStack = m_ItemStacks[originIndex];
            ItemStack destinationItemStack = destinationInventory.m_ItemStacks[destinationIndex];
            if (!originItemStack.Item) return false;

            if (originItemStack.Item.IsStackable && originItemStack.Item.Equals(destinationItemStack))
            {
                int remainder = destinationItemStack.AddAmount(originItemStack.Amount);

                if (remainder == 0)
                {
                    RemoveItemStack(originIndex);
                }
                else
                {
                    originItemStack.Amount = remainder;
                }
            }
            else
            {
                m_ItemStacks[originIndex] = destinationItemStack;
                destinationInventory.m_ItemStacks[destinationIndex] = originItemStack;
            }

            if (originIndex < NextEmptySlot && m_ItemStacks[originIndex].Item == null)
            {
                NextEmptySlot = originIndex;
            }
            if (destinationIndex == destinationInventory.NextEmptySlot)
            {
                destinationInventory.FindNextEmptySlot();
            }
            UpdateSlot(originIndex);
            destinationInventory.UpdateSlot(destinationIndex);
            StatsChanged = true;
            return true;
        }
        public void Clear()
        {
            for (int i = 0; i < m_ItemStacks.Count; i++)
            {
                RemoveItemStack(i);
            }
        }

        private void FindNextEmptySlot()
        {
            while (NextEmptySlot < m_ItemStacks.Count && !m_ItemStacks[NextEmptySlot].ItemIdentifier.Equals(""))
            {
                NextEmptySlot++;
            }
        }

        private void UpdateSlot(int slotToInsert)
        {
            m_OnSlotUpdate.Invoke(slotToInsert);
        }

        private int FindItemSlotIndexByIdentifier(string identifier)
        {
            for (int i = 0; i < m_ItemStacks.Count; i++)
            {
                ItemStack itemStack = m_ItemStacks[i];
                if (itemStack.Item != null &&
                    itemStack.Item.Identifier.Equals(identifier) &&
                    itemStack.Amount <= itemStack.Item.MaxStack)
                    return i;
            }
            return -1;
        }
        private void ResizeList()
        {
            int diff = m_ItemStacks.Count - m_Size;
            if (diff == 0) return;
            if (diff > 0)
            {
                m_ItemStacks.RemoveRange(m_Size, m_ItemStacks.Count - m_Size);
            }
            else
            {
                diff = -diff;
                for (int i = 0; i < diff; i++)
                {
                    m_ItemStacks.Add(new ItemStack("", 0));
                }
            }
        }

        public void OnValidate()
        {
            ResizeList();
            if (Application.isPlaying)
            {
                NextEmptySlot = 0;
                FindNextEmptySlot();
                for (int i = 0; i < m_ItemStacks.Count; i++)
                {
                    if (m_ItemStacks[i].Amount == 0)
                    {
                        RemoveItemStack(i);
                        continue;
                    }
                    m_ItemStacks[i].ForceUpdate();
                    UpdateSlot(i);
                }
            }
            StatsChanged = true;
        }

        #endregion
    }
}
