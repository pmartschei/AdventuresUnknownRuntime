using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class ActiveGemIdentifier : ObjectIdentifier
    {
        public new ActiveGem Object
        {
            get => base.Object as ActiveGem;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(ActiveGem) };
        }
    }
}
