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
        [SerializeField] private BasicModBaseIdentifier m_ModBaseIdentifier = new BasicModBaseIdentifier();
        [SerializeField] private float m_Value = 0;
        [SerializeField] private bool m_IsCurve = false;
        [SerializeField] private AnimationCurve m_ValueCurve = new AnimationCurve();

        #region Properties
        public string ModBaseIdentifier { get => m_ModBaseIdentifier.Identifier;}
        public BasicModBase ModBase { get => m_ModBaseIdentifier.Object; }
        public AnimationCurve ValueCurve { get => m_ValueCurve;}
        public bool IsCurve { get => m_IsCurve; set => m_IsCurve = value; }
        public float Value { get => m_Value; set => m_Value = value; }
        #endregion

        #region Methods

        public virtual bool ConsistencyCheck()
        {
            return m_ModBaseIdentifier.ConsistencyCheck();
        }

        public virtual float GetValue(float time)
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

        public Attribute Clone()
        {
            Attribute copy = new Attribute();
            copy.m_ModBaseIdentifier.Identifier = this.ModBaseIdentifier;
            copy.m_Value = this.m_Value;
            copy.IsCurve = this.IsCurve;
            Keyframe[] keyFrames = new Keyframe[m_ValueCurve.length];
            Keyframe[] originFrames = m_ValueCurve.keys;
            for(int i=0;i < keyFrames.Length;i++)
            {
                Keyframe keyframe = new Keyframe();

                keyframe.time = originFrames[i].time;
                keyframe.value = originFrames[i].value;
                keyframe.inTangent = originFrames[i].inTangent;
                keyframe.outTangent = originFrames[i].outTangent;
                keyframe.tangentMode = originFrames[i].tangentMode;
                keyframe.inWeight = originFrames[i].inWeight;
                keyframe.outWeight = originFrames[i].outWeight;
                keyframe.weightedMode = originFrames[i].weightedMode;

                keyFrames[i] = keyframe;
            }
            copy.m_ValueCurve.keys = keyFrames;
            return copy;
        }
        #endregion
    }
}
