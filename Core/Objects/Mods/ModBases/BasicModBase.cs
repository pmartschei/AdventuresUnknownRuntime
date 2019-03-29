using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.ModBases
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/ModBases/BasicModBase",fileName="BasicModBase.asset")]
    public class BasicModBase : CoreObject
    {
        [SerializeField] private ModTypeIdentifier m_ModTypeIdentifier;
        [SerializeField] private CalculationType m_CalculationType = CalculationType.Flat;
        
        #region Properties
        public CalculationType CalculationType
        {
            get => m_CalculationType;
            set {
                if (!m_CalculationType.Equals(value))
                {
                    m_CalculationType = value;
                    ForceUpdate();
                }
            }
        }
        public ModType ModType { get => m_ModTypeIdentifier.Object; }
        public string ModTypeIdentifier { get => m_ModTypeIdentifier.Identifier; }
        #endregion

        #region Methods

        public override bool ConsistencyCheck()
        {
            return m_ModTypeIdentifier.ConsistencyCheck();
        }
        public override bool IsIdentifierEditableInEditor()
        {
            return false;
        }
        protected override string IdentifierCalculation()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(m_ModTypeIdentifier.Identifier);
            sb.Append(".");
            sb.Append(CalculationType);
            return sb.ToString().ToLowerInvariant();
        }
        public void OnValidate()
        {
            ForceUpdateImmediately();
        }
        public virtual string ToText(string formatterIdentifier, float value)
        {
            return m_ModTypeIdentifier.Object.ToText(formatterIdentifier, value, m_CalculationType);
        }
        #endregion
    }
}
