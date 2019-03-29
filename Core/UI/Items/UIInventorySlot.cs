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
    public class UIInventorySlot : MonoBehaviour, IInventorySlot
    {
        [SerializeField] private InventoryIdentifier m_Inventory = null;
        [SerializeField] private int m_Slot = 0;
        [SerializeField] private AbstractItemStackDisplay m_ItemStackDisplay = null;

        [NonSerialized]
        private CanvasGroup m_CanvasGroup;

        #region Properties
        public Inventory Inventory
        {
            get { return m_Inventory.Object; }
        }
        public int Slot
        {
            get { return m_Slot; }
        }
        #endregion

        #region Methods

        private void Awake()
        {
            m_CanvasGroup = GetComponent<CanvasGroup>();
        }
        private void Start()
        {
            if (!m_Inventory.ConsistencyCheck() || m_Slot < 0 || m_Slot >= m_Inventory.Object.Size)
            {
                m_CanvasGroup.alpha = 0.5f;
                m_CanvasGroup.interactable = false;
                m_CanvasGroup.blocksRaycasts = false;
            }
            else
            {
                m_CanvasGroup.Show();
                Inventory.OnSlotUpdateEvent.AddListener(OnSlotUpdate);
                OnSlotUpdate(m_Slot);
            }
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