using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Datas;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class HotkeyDataIdentifier : ObjectIdentifier
    {
        public new HotkeyData Object
        {
            get => base.Object as HotkeyData;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(HotkeyData) };
        }
    }
}
