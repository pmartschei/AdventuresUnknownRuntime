using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Localization
{
    [CreateAssetMenu(menuName="AdventuresUnknown/Core/Localization/LocalizationLanguage",fileName="LocalizationLanguage.asset")]
    public class LocalizationLanguage : CoreObject
    {
        [SerializeField] private LocalizationString m_LanguageName = null;

        private bool m_CultureLanguageDataChanged = false;
        private string m_CultureLanguageName = "";


        #region Properties
        public string LanguageName { get => m_LanguageName.LocalizedString; }
        public string CultureLanguageName {
            get
            {
                UpdateIfChanged();
                if (m_CultureLanguageName.Contains(m_LanguageName.LocalizedIdentifier)) return LanguageName;
                return m_CultureLanguageName;
            }
        }
        #endregion

        #region Methods
        public override void ForceUpdate()
        {
            base.ForceUpdate();
            m_LanguageName.ForceUpdate();
            m_CultureLanguageDataChanged = true;
        }
        public void UpdateIfChanged()
        {
            if (m_CultureLanguageDataChanged)
            {
                m_CultureLanguageName = LocalizationsManager.Localize(m_LanguageName.LocalizedIdentifier, this);
                m_CultureLanguageDataChanged = false;
            }
        }
        #endregion
    }
}
