using AdventuresUnknownSDK.Core.Objects.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Currencies
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Currencies/Currency", fileName = "Currency.asset")]
    public class Currency : CoreObject
    {
        [SerializeField] private LocalizationString m_CurrencyName = null;
        [SerializeField] private Color m_TextColor = Color.white;
        [SerializeField] private Sprite m_Icon = null;

        #region Properties
        public Color TextColor { get => m_TextColor; set => m_TextColor = value; }
        public Sprite Icon { get => m_Icon; set => m_Icon = value; }
        public LocalizationString CurrencyName { get => m_CurrencyName; set => m_CurrencyName = value; }
        #endregion

        #region Methods

        #endregion
    }
}
