using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class ModTypeIdentifier : ObjectIdentifier
    {
        public new ModType Object
        {
            get => base.Object as ModType;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(ModType) };
        }
    }
}
