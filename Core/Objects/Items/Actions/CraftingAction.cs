using AdventuresUnknownSDK.Core.Objects.Currencies;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items.Interfaces;
using AdventuresUnknownSDK.Core.Objects.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Items.Actions
{
    [Serializable]
    public class CraftingAction
    {
        [SerializeField] private LocalizationString m_CraftingText = null;
        [SerializeField] private AbstractCosts m_Costs = null;
        [SerializeField] private AbstractEnabler m_Enabler = null;
        [SerializeField] private AbstractInvoker m_Invoker = null;


        #region Properties
        public string CraftingText { get => m_CraftingText.LocalizedString; }
        #endregion

        #region Methods
        public void ForceUpdate()
        {
            m_CraftingText.ForceUpdate();
        }
        public CurrencyValue[] GetCosts(ItemStack itemStack)
        {
            if (!m_Costs) return null;
            return m_Costs.GetCosts(itemStack);
        }
        public bool IsEnabled(ItemStack itemStack)
        {
            if (!m_Enabler) return true;
            return m_Enabler.IsEnabled(itemStack);
        }
        public void Invoke(ItemStack itemStack)
        {
            if (!m_Invoker) return;
            m_Invoker.Invoke(itemStack);
        }
        #endregion
    }
}
