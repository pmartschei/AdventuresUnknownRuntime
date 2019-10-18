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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Costs/ScaledCosts", fileName = "ScaledCosts.asset")]
    public class ScaledCosts : AbstractCosts
    {
        [SerializeField] private CurrencyValue[] m_Costs = null;
        [SerializeField] private bool m_UseItemValues = false;
        [SerializeField] private bool m_ScaleByImplicitCount = false;
        [SerializeField] private bool m_ScaleByExplicitCount = false;
        [SerializeField] private bool m_ScaleByPowerLevel = false;
        [SerializeField] private float m_Multiplier = 1.0f;

        #region Methods

        public override CurrencyValue[] GetCosts(ItemStack itemStack)
        {
            List<CurrencyValue> validCosts = new List<CurrencyValue>();
            if (!m_UseItemValues)
            {
                foreach (CurrencyValue cost in m_Costs)
                {
                    if (cost.Currency.ConsistencyCheck())
                    {
                        validCosts.Add(cost);
                    }
                }
            }
            else
            {
                validCosts.Add(itemStack.Item.CurrencyValue);
            }

            float multiplier = 1.0f;

            if (m_ScaleByExplicitCount)
            {
                multiplier += itemStack.ExplicitMods.Length;
            }
            if (m_ScaleByImplicitCount)
            {
                multiplier += itemStack.ImplicitMods.Length;
            }
            if (m_ScaleByPowerLevel)
            {
                multiplier += itemStack.PowerLevel;
            }
            multiplier *= m_Multiplier;
            for(int i = 0; i < validCosts.Count; i++)
            {
                CurrencyValue cv = validCosts[i];
                cv.Value = (long)(cv.Value * multiplier);
                validCosts[i] = cv;
            }
            return validCosts.ToArray();
        }
        #endregion
    }
}
