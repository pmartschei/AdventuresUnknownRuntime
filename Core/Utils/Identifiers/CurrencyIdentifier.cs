using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class CurrencyIdentifier : ObjectIdentifier
    {
        public new Currency Object{
            get => base.Object as Currency;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(Currency) };
        }
    }
}
