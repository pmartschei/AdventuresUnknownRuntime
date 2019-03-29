using AdventuresUnknownSDK.Core.Attributes;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods
{
    [Serializable]
    public class Attribute
    {
        [HideInInspector] [SerializeField] private string m_InspectorElementName = "Element";
        [SerializeField] private BasicModBaseIdentifier m_ModBaseIdentifier = null;
        [SerializeField] private float m_Value = 0;
        [SerializeField] private bool m_IsCurve = false;
        [SerializeField] private AnimationCurve m_ValueCurve;

        #region Properties
        public string ModBaseIdentifier { get => m_ModBaseIdentifier.Identifier; }
        public BasicModBase ModBase { get => m_ModBaseIdentifier.Object; }
        #endregion

        #region Methods

        public virtual bool ConsistencyCheck()
        {
            return m_ModBaseIdentifier.ConsistencyCheck();
        }

        public virtual float Value(float time)
        {
            if (m_IsCurve)
            {
                return m_ValueCurve.Evaluate(time);
            }
            return m_Value;
        }

        public void UpdateInspectorElementName()
        {
            m_InspectorElementName = m_ModBaseIdentifier.Identifier;
            if (m_InspectorElementName.Equals(string.Empty))
            {
                m_InspectorElementName = "EMPTY STRING";
            }
        }
        #endregion
    }
}
