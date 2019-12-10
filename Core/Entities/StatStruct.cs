using AdventuresUnknownSDK.Core.Objects.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities
{
    public struct StatStruct
    {
        private ModType m_ModType;
        private float m_Flat;
        private float m_Increased;
        private float m_More;
        private float m_FlatExtra;
        private float m_Calculated;
        private float m_CalculatedNoExtra;
        private float m_Current;
        private float m_Percentage;
        private bool m_IsDirty;
        private List<StatModifierStruct> m_StatModifiers;

        public ModType ModType { get => m_ModType; set => m_ModType = value; }
        public float Flat {
            get
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                return m_Flat;
            }
        }
        public float Increased {
            get
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                return m_Increased;
            }
        }
        public float More {
            get
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                return m_More;
            }
        }
        public float FlatExtra {
            get
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                return m_FlatExtra;
            }
        }
        public float Calculated {
            get
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                return m_Calculated;
            }
        }
        public float CalculatedNoExtra {
            get
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                return m_CalculatedNoExtra;
            }
        }
        public float Percentage {
            get
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                return m_Percentage;
            }
            set
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                float newValue = value;
                float minValue = float.MinValue;
                float maxValue = float.MaxValue;
                bool canGetHigherThanCalc = false;
                bool alwaysTakeMax = false;
                if (m_ModType != null)
                {
                    alwaysTakeMax = m_ModType.AlwaysTakeMax;
                    minValue = m_ModType.MinValue;
                    maxValue = m_ModType.MaxValue;
                    canGetHigherThanCalc = m_ModType.CanGetHigherThanCalculated;
                }
                if (alwaysTakeMax)
                {
                    newValue = 1;
                }
                else
                {
                    if (!canGetHigherThanCalc)
                    {
                        newValue = Mathf.Clamp(newValue, 0, 1);
                    }
                }
                float calculatedCurrentValue = m_Calculated * newValue;
                if (calculatedCurrentValue != m_Current)
                {
                    m_Current = calculatedCurrentValue;
                    m_Percentage = m_Calculated == 0.0f? 0.0f : newValue;
                }
            }
        }
        public float Current {
            get
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                return m_Current;
            }
            set
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                float newValue = value;
                float minValue = float.MinValue;
                float maxValue = float.MaxValue;
                bool canGetHigherThanCalc = false;
                bool alwaysTakeMax = false;
                if (m_ModType != null)
                {
                    alwaysTakeMax = m_ModType.AlwaysTakeMax;
                    minValue = m_ModType.MinValue;
                    maxValue = m_ModType.MaxValue;
                    canGetHigherThanCalc = m_ModType.CanGetHigherThanCalculated;
                }
                if (alwaysTakeMax)
                {
                    newValue = m_Calculated;
                }
                else
                {
                    if (canGetHigherThanCalc)
                    {
                        newValue = Mathf.Clamp(newValue, minValue, maxValue);
                    }
                    else
                    {
                        newValue = Mathf.Clamp(newValue, minValue, m_Calculated);
                    }
                }
                if (newValue != m_Current)
                {
                    m_Current = newValue;
                    if (m_Calculated != 0.0f)
                    {
                        m_Percentage = m_Current / m_Calculated;
                    }
                    else
                    {
                        m_Percentage = 0.0f;
                    }
                }
            }
        }

        public bool IsDirty { get => m_IsDirty; set => m_IsDirty = value; }

        public bool AddStatModifier(StatModifierStruct statModifierStruct)
        {
            if (m_StatModifiers == null)
            {
                m_StatModifiers = new List<StatModifierStruct>();
            }
            if (m_StatModifiers.Contains(statModifierStruct)) return false;
            m_StatModifiers.Add(statModifierStruct);
            m_IsDirty = true;
            return true;
        }

        public bool RemoveStatModifier(StatModifierStruct statModifierStruct)
        {
            if (m_StatModifiers == null) return false;
            bool removed = m_StatModifiers.Remove(statModifierStruct);
            if (removed)
            {
                m_IsDirty = true;
            }
            return removed;
        }
        public void RemoveStatModifiersBySource(object source)
        {
            StatModifierStruct[] statModifiers = GetStatModifiersBySource(source);
            foreach (StatModifierStruct statModifier in statModifiers)
            {
                RemoveStatModifier(statModifier);
            }
        }
        public StatModifierStruct[] GetStatModifiersBySource(object source)
        {
            List<StatModifierStruct> statModifiers = new List<StatModifierStruct>();
            foreach (StatModifierStruct statModifier in m_StatModifiers)
            {
                if (statModifier.Source == source) statModifiers.Add(statModifier);
            }
            return statModifiers.ToArray();
        }
        public void RemoveAllStatModifiers()
        {
            if (m_StatModifiers.Count == 0) return;
            m_StatModifiers.Clear();
            m_IsDirty = true;
        }
        private void Reset()
        {
            m_Flat = 0.0f;
            m_Increased = 1.0f;
            m_More = 1.0f;
            m_FlatExtra = 0.0f;
            m_Calculated = 0.0f;
            m_CalculatedNoExtra = 0.0f;
        }
        public void AddValue(float value, CalculationType calculationType)
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
                    AddMore(value + 1.0f);
                    break;
                case CalculationType.FlatExtra:
                    AddFlatExtra(value);
                    break;
            }
        }
        private void AddFlat(float flatValue)
        {
            this.m_Flat += flatValue;
        }
        private void AddIncreased(float increasedValue)
        {
            this.m_Increased += increasedValue;
        }
        private void AddMore(float moreValue)
        {
            this.m_More *= moreValue;
        }
        private void AddFlatExtra(float extraValue)
        {
            this.m_FlatExtra += extraValue;
        }
        private void Recalculate()
        {
            Reset();
            foreach (StatModifierStruct statModifier in m_StatModifiers)
            {
                AddValue(statModifier.Value, statModifier.CalculationType);
            }
            float minValue = float.MinValue;
            float maxValue = float.MaxValue;
            bool alwaysTakeMax = false;
            bool roundDown = false;
            bool canGetHigherThanCalc = false;
            if (m_ModType != null)
            {
                minValue = m_ModType.MinValue;
                maxValue = m_ModType.MaxValue;
                alwaysTakeMax = m_ModType.AlwaysTakeMax;
                roundDown = m_ModType.RoundDown;
                canGetHigherThanCalc = m_ModType.CanGetHigherThanCalculated;
            }
            float newCalculated = Mathf.Clamp(m_Flat * m_Increased * m_More, minValue, maxValue);
            if (newCalculated != m_CalculatedNoExtra)
            {
                m_CalculatedNoExtra = newCalculated;
            }
            newCalculated += m_FlatExtra;
            if (roundDown)
                newCalculated = Mathf.Floor(newCalculated);
            if (newCalculated != m_Calculated)
            {
                m_Calculated = newCalculated;
            }
            if (alwaysTakeMax)
            {
                m_Current = m_Calculated;
            }
            else
            {
                if (canGetHigherThanCalc)
                {
                    m_Current = Mathf.Clamp(m_Current, minValue, maxValue);
                }
                else
                {
                    m_Current = Mathf.Clamp(m_Current, minValue, m_Calculated);
                }
            }
            if (m_Calculated != 0.0f)
            {
                m_Percentage = m_Current / m_Calculated;
            }
            else
            {
                m_Percentage = 0.0f;
            }
        }

        public StatStruct Copy()
        {
            StatStruct copy = new StatStruct();
            for(int i = 0; i < m_StatModifiers.Count; i++)
            {
                copy.AddStatModifier(m_StatModifiers[i]);
            }

            return copy;
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ModType.Identifier);
            sb.Append(": ");
            sb.Append(Flat);
            sb.Append(" * ");
            sb.Append(Increased);
            sb.Append(" * ");
            sb.Append(More);
            sb.Append(" + ");
            sb.Append(FlatExtra);
            sb.Append(" = ");
            sb.Append(Calculated);

            return sb.ToString();
        }
    }
}
