using AdventuresUnknownSDK.Core.Objects.Currencies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.UI.Interfaces
{
    public abstract class ICurrencyText : IGameText
    {
        #region Methods
        public abstract void SetCurrency(Currency currency);
        #endregion
    }
}
