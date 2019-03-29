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
    public class ModIdentifier : ObjectIdentifier
    {
        public new Mod Object
        {
            get => base.Object as Mod;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(Mod) };
        }
    }
}
