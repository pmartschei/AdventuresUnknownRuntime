using AdventuresUnknownSDK.Core.Objects.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventuresUnknownSDK.Core.Utils.Identifiers
{
    [Serializable]
    public class WalletIdentifier : ObjectIdentifier
    {
        public new Wallet Object
        {
            get => base.Object as Wallet;
        }
        public override Type[] GetSupportedTypes()
        {
            return new Type[] { typeof(Wallet) };
        }
    }
}
