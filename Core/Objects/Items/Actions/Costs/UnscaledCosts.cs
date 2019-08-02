using AdventuresUnknownSDK.Core.Objects.Currencies;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Items.Actions.Costs
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Costs/UnscaledCosts", fileName = "UnscaledCosts.asset")]
    public class UnscaledCosts : AbstractCosts
    {
        [SerializeField] private CurrencyValue[] m_Costs = null;
        #region Methods

        public override CurrencyValue[] GetCosts(ItemStack itemStack)
        {
            List<CurrencyValue> validCosts = new List<CurrencyValue>();
            foreach(CurrencyValue cost in m_Costs)
            {
                if (cost.Currency.ConsistencyCheck())
                {
                    validCosts.Add(cost);
                }
            }
            return validCosts.ToArray();
        }
        #endregion
    }
}
