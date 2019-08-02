using AdventuresUnknownSDK.Core.Objects.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Items.Interfaces
{
    public abstract class IInventorySlot : MonoBehaviour
    {
        public abstract Inventory Inventory { get; }
        public abstract int Slot { get; }
    }
}
