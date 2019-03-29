using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.EventSystems;

namespace AdventuresUnknownSDK.Core.UI.Items.Interfaces
{
    public interface IDragItemStack : IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler
    {
        IInventorySlot IInventorySlot { get; }
    }
}
