using AdventuresUnknownSDK.Core.Objects.Items.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class CraftingActionCatalogIdentifier : ObjectIdentifier
    {
        public new CraftingActionCatalog Object
        {
            get => base.Object as CraftingActionCatalog;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(CraftingActionCatalog) };
        }
    }
}
