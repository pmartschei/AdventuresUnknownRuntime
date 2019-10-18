using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.EventSystems;

namespace AdventuresUnknownSDK.Core.UI.Interfaces
{
    public interface IDragOverHandler : IEventSystemHandler
    {
        void OnDragOver(PointerEventData eventData);
    }
}
