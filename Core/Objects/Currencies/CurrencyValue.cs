using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Currencies
{
    [Serializable]
    public class CurrencyValue
    {
        [SerializeField] private CurrencyIdentifier m_Currency = new CurrencyIdentifier();
        [SerializeField] private long m_Value = 0;

        #region Properties
        public CurrencyIdentifier Currency { get => m_Currency; set => m_Currency = value; }
        public long Value { get => m_Value; set => m_Value = value; }
        #endregion

        #region Methods

        #endregion
    }
}
