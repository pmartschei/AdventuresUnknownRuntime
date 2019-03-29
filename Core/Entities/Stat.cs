using AdventuresUnknownSDK.Core.Attributes;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities
{
    [Serializable]
    public class Stat
    {
        [LowerCaseOnly]
        [SerializeField] private ModType m_ModType;
        [SerializeField] private float m_Flat = 0.0f;
        [SerializeField] private float m_Increased = 1.0f;
        [SerializeField] private float m_More = 1.0f;
        [SerializeField] private float m_Calculated = 0.0f;
        [SerializeField] private float m_Current = 0.0f;
        private bool m_Update = false;

        public List<StatModifier> statModifiers = new List<StatModifier>();
        
        #region Properties
        public string ModTypeIdentifier { get => m_ModType == null ? "" : m_ModType.Identifier; }
        public ModType ModType { get => m_ModType; }
        
        public float Increased
        {
            get {
                return m_Increased;
            }
        }
        public float More
        {
            get
            {
                return m_More;
            }
        }
        public float Flat
        {
            get
            {
                return m_Flat;
            }
        }
        public float Calculated
        {
            get
            {
                if (m_Update)
                {
                    Recalculate();
                    m_Update = false;
                }
                return m_Calculated;
            }
        }
        public float Current { get => m_Current; set => m_Current = Mathf.Min(value,m_Calculated); }
        #endregion

        #region Methods
        public Stat(ModType modType)
        {
            m_ModType = modType;
            Reset();
        }
        protected virtual void Reset()
        {
            m_Flat = 0.0f;
            m_Increased = 1.0f;
            m_More = 1.0f;
            m_Calculated = 0.0f;
        }

        protected virtual void Recalculate()
        {
            float minValue = float.MinValue;
            float maxValue = float.MaxValue;
            bool alwaysTakeMax = false;
            if (m_ModType != null)
            {
                minValue = m_ModType.MinValue;
                maxValue = m_ModType.MaxValue;
                alwaysTakeMax = m_ModType.AlwaysTakeMax;
            }
            m_Calculated = Mathf.Clamp(m_Flat * m_Increased * m_More, minValue, maxValue);
            if (alwaysTakeMax)
            {
                m_Current = m_Calculated;
            }
            else
            {
                m_Current = Mathf.Clamp(minValue, Current, m_Calculated);
            }
        }

        public void AddValue(float value,CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.Flat:
                    AddFlat(value);
                    break;
                case CalculationType.Increased:
                    AddIncreased(value);
                    break;
                case CalculationType.More:
                    AddMore(value+1.0f);
                    break;
            }
        }

        protected virtual void AddCurrent(float currentValue)
        {
            this.m_Current += currentValue;
        }

        protected virtual void AddFlat(float flatValue)
        {
            this.m_Flat += flatValue;
            m_Update = true;
        }
        protected virtual void AddIncreased(float increasedValue)
        {
            this.m_Increased += increasedValue;
            m_Update = true;
        }
        protected virtual void AddMore(float moreValue)
        {
            this.m_More *= moreValue;
            m_Update = true;
        }
        public static Stat operator+(Stat s1,Stat s2)
        {
            s1.AddFlat(s2.Flat);
            s1.AddIncreased(s2.Increased-1.0f);
            s1.AddMore(s2.More);
            return s1;
        }

        #endregion
    }
}
