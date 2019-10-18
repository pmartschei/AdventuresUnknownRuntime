using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Effects;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class EffectIdentifier : ObjectIdentifier
    {
        public new Effect Object
        {
            get => base.Object as Effect;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(Effect) };
        }
    }
}
