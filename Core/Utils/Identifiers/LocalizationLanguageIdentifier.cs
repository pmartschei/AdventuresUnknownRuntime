using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
using AdventuresUnknownSDK.Core.Objects.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AdventuresUnknownSDK.Core.Objects;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class LocalizationLanguageIdentifier : ObjectIdentifier
    {
        public new LocalizationLanguage Object
        {
            get => base.Object as LocalizationLanguage;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(LocalizationLanguage) };
        }
    }
}
