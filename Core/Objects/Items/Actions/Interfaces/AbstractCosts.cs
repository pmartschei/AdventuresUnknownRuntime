using AdventuresUnknownSDK.Core.Objects.Currencies;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Items.Interfaces
{
    public abstract class AbstractCosts : ScriptableObject
    {
        #region Methods
        public abstract CurrencyValue[] GetCosts(ItemStack itemStack);
        #endregion
    }
}
