using AdventuresUnknownSDK.Core.UI.Interfaces;
using AdventuresUnknownSDK.Core.UI.Items.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AdventuresUnknownSDK.Core.UI.Items
{
    [AddComponentMenu("AdventuresUnknown/UI/UIDragDropItemStack")]
    public class UIDragDropItemStack : MonoBehaviour, IDragItemStack, IDropHandler,IDropPreviewHandler
    {
        [SerializeField] private CanvasGroup m_CanvasGroup = null;
        [SerializeField] private CanvasGroup m_DragObject = null;
        [SerializeField] private UIInventorySlot m_UIInventorySlot = null;
        [SerializeField] private UIDropItemStackFilter m_ItemStackFilter = null;
        [SerializeField] private IPreview m_Preview = null;

        private Transform m_LastParent = null;
        private Canvas m_Root = null;
        private bool m_IsDragging = false;
        


        #region Properties

        public IInventorySlot IInventorySlot
        {
            get { return m_UIInventorySlot; }
        }

        public UIDropItemStackFilter ItemStackFilter { get => m_ItemStackFilter; }

        #endregion

        #region Methods
        private void Awake()
        {
            if (!m_DragObject || !m_UIInventorySlot)
                Debug.LogError("UIDragItemStack an input is null", this);
        }

        private void OnDisable()
        {
            if (m_IsDragging)
            {
                OnEndDrag(null);
            }
        }
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
            m_IsDragging = true;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            if (!m_IsDragging) return;
            if (m_CanvasGroup)
                m_CanvasGroup.blocksRaycasts = false;
            if (!IInventorySlot.Inventory.Items[IInventorySlot.Slot].IsEmpty)
                m_DragObject.blocksRaycasts = false;
            m_Root = m_DragObject.transform.root.GetComponent<Canvas>();
            m_LastParent = m_DragObject.transform.parent;
            m_DragObject.transform.SetParent(m_Root.transform, true);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            if (!m_IsDragging || eventData.button != PointerEventData.InputButton.Left || IInventorySlot.Inventory.Items[IInventorySlot.Slot].IsEmpty)
            {
                eventData.dragging = false;
                OnEndDrag(eventData);
                return;
            }
            if (m_Root.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                m_DragObject.transform.position = eventData.position;
            }
            else// if (m_Root.renderMode == RenderMode.ScreenSpaceCamera)
            {
                Vector3 oldZ = m_DragObject.transform.position;
                Vector3 newVector = m_Root.worldCamera.ScreenToWorldPoint(eventData.position);
                newVector.z = oldZ.z;
                m_DragObject.transform.position = newVector;
            }
        }

        public virtual void OnEndDrag(PointerEventData eventData)
        {
            m_IsDragging = false;
            if (m_CanvasGroup)
                m_CanvasGroup.blocksRaycasts = true;
            if (!IInventorySlot.Inventory.Items[IInventorySlot.Slot].IsEmpty)
                m_DragObject.blocksRaycasts = true;
            m_DragObject.transform.SetParent(m_LastParent);

        }

        public virtual void OnEndDropPreview(PointerEventData eventData)
        {
            if (m_Preview)
            {
                m_Preview.Hide();
            }
        }
        public virtual void OnDropPreview(PointerEventData eventData)
        {
            if (m_Preview == null) return;
            IDragItemStack iDragItemStack = eventData.pointerDrag.GetComponent<IDragItemStack>();
            if (iDragItemStack == null)
            {
                m_Preview.Hide();
                return;
            }
            IInventorySlot originIS = iDragItemStack.IInventorySlot;
            IInventorySlot destinationIS = this.IInventorySlot;
            if (originIS == destinationIS || originIS.Inventory.Items[originIS.Slot].IsEmpty)
            {
                m_Preview.Hide();
                return;
            }
            if (m_ItemStackFilter)
            {
                if (!m_ItemStackFilter.Filter(originIS.Inventory.Items[originIS.Slot]))
                {
                    m_Preview.Show(false);
                    return;
                }
            }
            if (iDragItemStack.ItemStackFilter)
            {
                if (!iDragItemStack.ItemStackFilter.Filter(destinationIS.Inventory.Items[destinationIS.Slot]))
                {
                    m_Preview.Show(false);
                    return;
                }
            }
            m_Preview.Show(true);
        }
        public virtual void OnDrop(PointerEventData eventData)
        {
            IDragItemStack iDragItemStack = eventData.pointerDrag.GetComponent<IDragItemStack>();
            if (iDragItemStack == null) return;
            IInventorySlot originIS = iDragItemStack.IInventorySlot;
            IInventorySlot destinationIS = this.IInventorySlot;
            if (m_ItemStackFilter)
            {
                if (!m_ItemStackFilter.Filter(originIS.Inventory.Items[originIS.Slot]))
                {
                    return;
                }
            }
            if (iDragItemStack.ItemStackFilter)
            {
                if (!iDragItemStack.ItemStackFilter.Filter(destinationIS.Inventory.Items[destinationIS.Slot]))
                {
                    return;
                }
            }

            originIS.Inventory.SwitchItemStacks(originIS.Slot, destinationIS.Inventory, destinationIS.Slot);
            eventData.Use();
        }
        
        #endregion
    }
}
