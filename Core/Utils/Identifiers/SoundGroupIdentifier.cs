using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
using AdventuresUnknownSDK.Core.Objects.Sounds;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class SoundGroupIdentifier : ObjectIdentifier
    {
        public new SoundGroup Object
        {
            get => base.Object as SoundGroup;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(SoundGroup) };
        }
    }
}
