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
    public class UIDragDropItemStack : MonoBehaviour, IDragItemStack, IDropHandler
    {
        [SerializeField] private CanvasGroup m_DragObject = null;
        [SerializeField] private UIInventorySlot m_UIInventorySlot = null;

        private Transform m_LastParent = null;
        private Canvas m_Root = null;

        #region Properties

        public IInventorySlot IInventorySlot
        {
            get { return m_UIInventorySlot; }
        }

        #endregion

        #region Methods
        private void Awake()
        {
            if (!m_DragObject || !m_UIInventorySlot)
                Debug.LogError("UIDragItemStack an input is null", this);
        }
        public virtual void OnPointerDown(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
        }

        public virtual void OnBeginDrag(PointerEventData eventData)
        {
            m_DragObject.blocksRaycasts = false;
            m_Root = m_DragObject.transform.root.GetComponent<Canvas>();
            m_LastParent = m_DragObject.transform.parent;
            m_DragObject.transform.SetParent(m_Root.transform, true);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
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
            m_DragObject.transform.SetParent(m_LastParent);
        }
        public virtual void OnDrop(PointerEventData eventData)
        {
            IDragItemStack iDragItemStack = eventData.pointerDrag.GetComponent<IDragItemStack>();

            if (iDragItemStack == null) return;
            IInventorySlot originIS = iDragItemStack.IInventorySlot;
            IInventorySlot destinationIS = this.IInventorySlot;
            originIS.Inventory.SwitchItemStacks(originIS.Slot, destinationIS.Inventory, destinationIS.Slot);
            eventData.Use();
        }

        #endregion
    }
}
