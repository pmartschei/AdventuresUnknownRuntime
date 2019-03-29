using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Localization
{
    [CreateAssetMenu(menuName="AdventuresUnknown/Core/Localization/LocalizationTable",fileName="LocalizationTable.asset")]
    public class LocalizationTable : ScriptableObject
    {
        [Serializable]
        public struct IdentifierString
        {
            [SerializeField] private string m_Identifier;
            [SerializeField] private string m_String;

            public string Identifier { get => m_Identifier; }
            public string String { get => m_String; }
        }
        [SerializeField] private LocalizationLanguageIdentifier m_LanguageIdentifier = null;
        [SerializeField] private IdentifierString[] m_LocalizationData = null;


        #region Properties
        public LocalizationLanguage Language { get => m_LanguageIdentifier.Object; }
        public IdentifierString[] LocalizationData { get => m_LocalizationData;}
        #endregion

        #region Methods
        public bool ConsistencyCheck()
        {
            return m_LanguageIdentifier.ConsistencyCheck();
        }
        #endregion
    }
}
