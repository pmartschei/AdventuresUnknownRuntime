using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Utils;
using AdventuresUnknownSDK.Core.Utils.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Localization
{
    public class UILocalizationText : LocalizedMonoBehaviour
    {
        [SerializeField] private LocalizationString m_LocalizationString = null;
        [SerializeField] private StringEvent m_OnLocalize = null;
        #region Properties

        #endregion

        #region Methods
        private void Awake()
        {
            OnLanguageChange();
        }
        public override void OnLanguageChange()
        {
            m_LocalizationString.ForceUpdate();
            m_OnLocalize.Invoke(m_LocalizationString.LocalizedString);
        }
        #endregion
    }
}
