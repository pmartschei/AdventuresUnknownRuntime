using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.UI.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI
{
    public class UILocalizedGameText : UIGameText
    {
        [SerializeField] private LocalizationString m_LocalizationString = null;

        private object m_Object = null;
        #region Properties

        #endregion

        #region Methods

        private void OnEnable()
        {
            GameSettingsManager.OnLanguageChange.AddListener(OnLanguageChange);
            OnLanguageChange();
        }

        private void OnDisable()
        {
            GameSettingsManager.OnLanguageChange.RemoveListener(OnLanguageChange);
        }

        private void OnLanguageChange()
        {
            m_LocalizationString.ForceUpdate();
            SetText(m_Object);
        }

        public override void SetText(object obj)
        {
            m_Object = obj;
            if (!Text) return;
            string text = string.Format(m_LocalizationString.LocalizedString, obj);
            if (!Text.text.Equals(text))
            {
                Text.text = text;
                OnTextChange.Invoke(text);
            }
        }
        #endregion
    }
}
