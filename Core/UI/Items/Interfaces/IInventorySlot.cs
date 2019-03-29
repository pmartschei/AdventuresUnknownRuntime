using AdventuresUnknownSDK.Core.Objects.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.UI.Items.Interfaces
{
    public interface IInventorySlot
    {
        Inventory Inventory { get; }
        int Slot { get; }
    }
}
