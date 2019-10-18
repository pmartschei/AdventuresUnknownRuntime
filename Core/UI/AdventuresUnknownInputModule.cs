using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.UI.Interfaces;
using AdventuresUnknownSDK.Core.Utils.Extensions;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.EventSystems.PointerEventData;

namespace AdventuresUnknownSDK.Core.UI
{
    public class AdventuresUnknownInputModule : StandaloneInputModule
    {

        private Dictionary<InputButton, bool> m_IsDragging = new Dictionary<InputButton, bool>();
        static AdventuresUnknownInputModule()
        {
            DragOverHandler = ExecuteDragOverHandler;
            DropPreviewHandler = ExecuteDropPreviewHandler;
            EndDropPreviewHandler = ExecuteEndDropPreviewHandler;
        }

        public static ExecuteEvents.EventFunction<IDragOverHandler> DragOverHandler { get; set; }
        public static ExecuteEvents.EventFunction<IDropPreviewHandler> DropPreviewHandler { get; set; }
        public static ExecuteEvents.EventFunction<IDropPreviewHandler> EndDropPreviewHandler { get; set; }
        protected override void ProcessDrag(PointerEventData pointerEvent)
        {
            base.ProcessDrag(pointerEvent);
            bool value;
            if (pointerEvent.dragging)
            {
                if (!m_IsDragging.TryGetValue(pointerEvent.button,out value))
                {
                    m_IsDragging.Add(pointerEvent.button, true);
                }
                else
                {
                    m_IsDragging[pointerEvent.button] = true;
                }
                GameObject currentOverGo = pointerEvent.pointerCurrentRaycast.gameObject;
                ExecuteEventsExtension.ExecuteHierarchyDown(pointerEvent.pointerDrag.transform.root.gameObject, pointerEvent, DropPreviewHandler);
                if (currentOverGo)
                {
                    ExecuteEvents.Execute(currentOverGo, pointerEvent, DragOverHandler);
                }
            }
            bool released = GetMousePointerEventData().GetButtonState(pointerEvent.button).eventData.ReleasedThisFrame();
            bool available = m_IsDragging.TryGetValue(pointerEvent.button, out value);
            if (released && available && value)
            {
                m_IsDragging[pointerEvent.button] = false;
                ExecuteEventsExtension.ExecuteHierarchyDown(pointerEvent.lastPress.transform.root.gameObject, pointerEvent, EndDropPreviewHandler);
            }
        }

        private static void ExecuteDragOverHandler(IDragOverHandler dragOverHandler, BaseEventData eventData)
        {
            dragOverHandler.OnDragOver(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
        }
        private static void ExecuteDropPreviewHandler(IDropPreviewHandler dropPreviewHandler, BaseEventData eventData)
        {
            dropPreviewHandler.OnDropPreview(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
        }
        private static void ExecuteEndDropPreviewHandler(IDropPreviewHandler dropPreviewHandler, BaseEventData eventData)
        {
            dropPreviewHandler.OnEndDropPreview(ExecuteEvents.ValidateEventData<PointerEventData>(eventData));
        }
    }
}
