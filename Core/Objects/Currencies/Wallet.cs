using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Currencies
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Currencies/Wallet", fileName = "Wallet.asset")]
    public class Wallet : CoreObject
    {

        [HideInInspector] [SerializeField] private bool m_IsFoldout = false;
        private Dictionary<string, long> m_DictionaryCurrencies = new Dictionary<string, long>();

        #region Properties
        public List<KeyValuePair<string, long>> CurrencyList { get => m_DictionaryCurrencies.ToList(); }

        #endregion

        #region Methods
        public void AddValue(string identifier, long value)
        {
            if (!m_DictionaryCurrencies.ContainsKey(identifier))
            {
                m_DictionaryCurrencies.Add(identifier, value);
            }
            else
            {
                long currentValue = m_DictionaryCurrencies[identifier];
                currentValue += value;
                m_DictionaryCurrencies[identifier] = currentValue;
            }
            PlayerManager.OnWalletDisplayChange.Invoke();
            //should be changed to own unityevent
        }
        public long GetValue(string identifier)
        {
            if (m_DictionaryCurrencies.ContainsKey(identifier))
            {
                return m_DictionaryCurrencies[identifier];
            }
            return 0;
        }
        public override void Initialize()
        {
            m_DictionaryCurrencies.Clear();
        }
        #endregion
    }
}
