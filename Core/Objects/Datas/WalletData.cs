using AdventuresUnknownSDK.Core.Objects.Currencies;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Datas/WalletData", fileName = "WalletData.asset")]
    public class WalletData : IPlayerData
    {
        [SerializeField] private WalletIdentifier m_WalletIdentifier = null;
        [SerializeField] [HideInInspector] private CurrencyValue[] m_Currencies = null;

        #region Properties

        #endregion

        #region Methods
        public override bool OnBeforeSerialize()
        {
            if (!m_WalletIdentifier.ConsistencyCheck()) return false;
            var currencyList = m_WalletIdentifier.Object.CurrencyList;
            m_Currencies = new CurrencyValue[currencyList.Count];
            int i = 0;
            foreach(var currency in currencyList)
            {
                m_Currencies[i] = new CurrencyValue();
                m_Currencies[i].Currency.Identifier = currency.Key;
                m_Currencies[i].Value = currency.Value;
                i++;
            }
            return true;
        }
        public override bool ConsistencyCheck()
        {
            return m_WalletIdentifier.ConsistencyCheck();
        }

        public override void Reset()
        {
            m_WalletIdentifier.Object.Initialize();
            m_Currencies = null;
        }

        public override void Load()
        {
            WalletData walletData = FindScriptableObject<WalletData>();
            if (!walletData) return;
            walletData.m_WalletIdentifier = this.m_WalletIdentifier;
            if (!walletData.m_WalletIdentifier.ConsistencyCheck()) return;
            walletData.m_WalletIdentifier.Object.Initialize();
            foreach (CurrencyValue currency in m_Currencies)
            {
                walletData.m_WalletIdentifier.Object.AddValue(currency.Currency.Identifier, currency.Value);
            }
            return;
        }

        #endregion
    }
}
