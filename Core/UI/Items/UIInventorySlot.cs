using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.UI.Items.Interfaces;
using AdventuresUnknownSDK.Core.Utils.Extensions;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Items
{
    [AddComponentMenu("AdventuresUnknown/UI/UIInventorySlot")]
    [RequireComponent(typeof(CanvasGroup))]
    public class UIInventorySlot : IInventorySlot
    {
        [SerializeField] private InventoryIdentifier m_Inventory = null;
        [SerializeField] private int m_Slot = 0;
        [SerializeField] private AbstractItemStackDisplay m_ItemStackDisplay = null;

        [NonSerialized]
        private CanvasGroup m_CanvasGroup;

        #region Properties
        public override Inventory Inventory
        {
            get { return m_Inventory.Object; }
        }
        public override int Slot
        {
            get { return m_Slot; }
        }
        #endregion

        #region Methods

        private void Start()
        {
            OnSlotUpdate(m_Slot);
        }

        private void OnEnable()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
            if (!m_Inventory.ConsistencyCheck() || m_Slot < 0 || m_Slot >= m_Inventory.Object.Size)
            {
                m_CanvasGroup.Hide();
            }
            else
            {
                m_CanvasGroup.Show();
                Inventory.OnSlotUpdateEvent.AddListener(OnSlotUpdate);
            }
        }

        private void OnDisable()
        {
            Inventory.OnSlotUpdateEvent.RemoveListener(OnSlotUpdate);
        }

        private void OnSlotUpdate(int slot)
        {
            if (slot != m_Slot) return;

            if (m_ItemStackDisplay!=null)
                m_ItemStackDisplay.Display(Inventory.Items[m_Slot]);
        }
        #endregion

    }
}