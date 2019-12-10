using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Currencies;
using AdventuresUnknownSDK.Core.UI.Interfaces;
using AdventuresUnknownSDK.Core.Utils.UnityEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace AdventuresUnknownSDK.Core.UI
{
    public class UICurrencyText : ICurrencyText
    {
        [SerializeField] private TMP_Text m_Text = null;
        [SerializeField] private Image m_CurrencyImage = null;

        [SerializeField] private StringEvent m_OnTextChange = null;

        private Currency m_CurrentCurrency = null;

        #region Properties

        #endregion

        #region Methods
        public override void SetText(object obj)
        {
            if (!m_Text) return;
            string text = Formatize(obj.ToString());
            if (!m_Text.text.Equals(text))
            {
                m_Text.text = text;
                m_OnTextChange.Invoke(text);
            }
        }
        public virtual string Formatize(string text)
        {
            long value = 0;
            if(long.TryParse(text, out value))
            {
                float decimalValue = value;
                int potenz = 0;
                while (decimalValue >= 1000)
                {
                    decimalValue /= 1000.0f;
                    potenz++;
                }
                int decimalPieces = 0;
                if (potenz > 0)
                {
                    if (decimalValue < 10)
                    {
                        decimalPieces = 2;
                    }
                    else if (decimalValue < 100)
                    {
                        decimalPieces = 1;
                    }
                }
                return string.Format("{0:F" + decimalPieces + "} {1}", decimalValue, GetLocalizationDecimal(potenz));
            }
            return text;
        }
        public virtual string GetLocalizationDecimal(int count)
        {
            return LocalizationsManager.Localize("core.ui.numeral" + count);
        }
        public override void SetCurrency(Currency currency)
        {
            m_CurrentCurrency = currency;
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (!m_CurrentCurrency) return;
            if (m_Text)
                m_Text.color = m_CurrentCurrency.TextColor;
            if (m_CurrencyImage)
                m_CurrencyImage.sprite = m_CurrentCurrency.Icon;
        }

        public override void SetColor(Color color)
        {
            if (!m_Text) return;
            m_Text.color = color;
        }
        #endregion
    }
}
