using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class InventoryIdentifier : ObjectIdentifier
    {
        public new Inventory Object
        {
            get => base.Object as Inventory;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(Inventory) };
        }
    }
}
