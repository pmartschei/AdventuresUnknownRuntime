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
    public class JourneyDataIdentifier : ObjectIdentifier
    {
        public new JourneyData Object
        {
            get => base.Object as JourneyData;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(JourneyData) };
        }
    }
}
