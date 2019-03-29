using AdventuresUnknownSDK.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Objects.Localization
{
    [Serializable]
    public class LocalizationString
    {
        [SerializeField] private string m_LocalizedIdentifier = "";

        [NonSerialized]
        private bool m_LocalizedIdentifierDataChanged = false;

        private string m_LocalizedString = "";
        public LocalizationString()
        {
        }
        public LocalizationString(string identifier)
        {
            m_LocalizedIdentifier = identifier;
        }
        #region Properties
        public string LocalizedString {
            get
            {
                UpdateIfChanged();
                return m_LocalizedString;
            }
        }

        public string LocalizedIdentifier {
            get => m_LocalizedIdentifier;
            set
            {
                if (!m_LocalizedIdentifier.Equals(value))
                {
                    ForceUpdate();
                    m_LocalizedIdentifier = value;
                }
            }
        }

    #endregion

    #region Methods
        private void OnGameLanguageChange()
        {
            ForceUpdate();
        }
        private void UpdateIfChanged()
        {
            if (m_LocalizedIdentifierDataChanged)
            {
                m_LocalizedString = LocalizationsManager.Localize(m_LocalizedIdentifier);
                m_LocalizedIdentifierDataChanged = false;
            }
        }

        public void ForceUpdate()
        {
            m_LocalizedIdentifierDataChanged = true;
        }
        public override string ToString()
        {
            return LocalizedString;
        }
    #endregion
    }
}
