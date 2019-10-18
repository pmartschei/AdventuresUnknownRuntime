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
    public class VendorDataIdentifier : ObjectIdentifier
    {
        public new VendorData Object
        {
            get => base.Object as VendorData;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(VendorData) };
        }
    }
}
