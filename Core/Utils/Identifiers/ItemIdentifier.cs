using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class ItemIdentifier : ObjectIdentifier
    {
        public new Item Object
        {
            get => base.Object as Item;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(Item) };
        }
    }
}
