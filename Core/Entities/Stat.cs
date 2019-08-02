using AdventuresUnknownSDK.Core.Attributes;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Entities
{
    [Serializable]
    public class Stat
    {
        [SerializeField] private ModType m_ModType;
        [SerializeField] private float m_Flat = 0.0f;
        [SerializeField] private float m_Increased = 1.0f;
        [SerializeField] private float m_More = 1.0f;
        [SerializeField] private float m_FlatExtra = 0.0f;
        [SerializeField] private float m_Calculated = 0.0f;
        [SerializeField] private float m_CalculatedNoExtra = 0.0f;
        [SerializeField] private float m_Current = 0.0f;
        [SerializeField] private float m_Percentage = 0.0f;
        private bool m_IsDirty = false;

        public List<StatModifier> m_StatModifiers = new List<StatModifier>();

        #region Properties
        public string ModTypeIdentifier { get => m_ModType == null ? "" : m_ModType.Identifier; }
        public ModType ModType { get => m_ModType; }

        public float Increased
        {
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
        public float More
        {
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
        public float Flat
        {
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
        public float FlatExtra
        {
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
        public float Percentage
        {
            get
            {
                if (IsDirty)
                {
                    Recalculate();
                    IsDirty = false;
                }
                return m_Percentage;
            }
        }
        public float Calculated
        {
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
        public float CalculatedNoExtra
        {
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
        public float Current
        {
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
                if (m_ModType != null && m_ModType.AlwaysTakeMax)
                {
                    newValue = m_Calculated;
                    minValue = m_ModType.MinValue;
                }
                else
                {
                    newValue = Mathf.Clamp(newValue, minValue, m_Calculated);
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
                    StatChanged = true;
                }
            }
        }

        public bool StatChanged { get; set; }
        public bool IsDirty
        {
            get
            {
                if (m_IsDirty) return true;
                foreach(StatModifier statModifier in m_StatModifiers)
                {
                    if (statModifier.IsDirty) return true;
                }
                return false;
            }
            set
            {
                m_IsDirty = value;
                foreach(StatModifier statModifier in m_StatModifiers)
                {
                    statModifier.IsDirty = value;
                }
            }
        }
        #endregion

        #region Methods
        public void AddStatModifier(StatModifier statModifier)
        {
            if (m_StatModifiers.Contains(statModifier)) return;
            m_StatModifiers.Add(statModifier);
            m_IsDirty = true;
            StatChanged = true;
        }
        public bool RemoveStatModifier(StatModifier statModifier)
        {
            bool removed = m_StatModifiers.Remove(statModifier);
            if (removed)
            {
                m_IsDirty = true;
                StatChanged = true;
            }
            return removed;
        }
        public void RemoveStatModifiersBySource(object source)
        {
            StatModifier[] statModifiers = GetStatModifiersBySource(source);
            foreach(StatModifier statModifier in statModifiers)
            {
                RemoveStatModifier(statModifier);
            }
        }
        public StatModifier[] GetStatModifiersBySource(object source)
        {
            List<StatModifier> statModifiers = new List<StatModifier>();
            foreach(StatModifier statModifier in m_StatModifiers)
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
        public Stat(ModType modType)
        {
            m_ModType = modType;
            Recalculate();
        }
        protected virtual void Reset()
        {
            m_Flat = 0.0f;
            m_Increased = 1.0f;
            m_More = 1.0f;
            m_FlatExtra = 0.0f;
            m_Calculated = 0.0f;
        }

        protected virtual void Recalculate()
        {
            Reset();
            foreach(StatModifier statModifier in m_StatModifiers)
            {
                AddValue(statModifier.Value, statModifier.CalculationType);
            }
            float minValue = float.MinValue;
            float maxValue = float.MaxValue;
            bool alwaysTakeMax = false;
            bool roundDown = false;
            if (m_ModType != null)
            {
                minValue = m_ModType.MinValue;
                maxValue = m_ModType.MaxValue;
                alwaysTakeMax = m_ModType.AlwaysTakeMax;
                roundDown = m_ModType.RoundDown;
            }
            float newCalculated = Mathf.Clamp(m_Flat * m_Increased * m_More, minValue, maxValue);
            if (newCalculated != m_CalculatedNoExtra)
            {
                m_CalculatedNoExtra = newCalculated;
                StatChanged = true;
            }
            newCalculated += m_FlatExtra;
            if (roundDown)
                newCalculated = Mathf.Floor(newCalculated);
            if (newCalculated != m_Calculated)
            {
                m_Calculated = newCalculated;
                StatChanged = true;
            }
            if (alwaysTakeMax)
            {
                m_Current = m_Calculated;
            }
            else
            {
                m_Current = Mathf.Clamp(m_Current, minValue, m_Calculated);
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

        public float GetValue(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.Flat:
                    return Flat;
                case CalculationType.Increased:
                    return Increased-1.0f;
                case CalculationType.More:
                    return More-1.0f;
                case CalculationType.Calculated:
                    return Calculated;
                case CalculationType.FlatExtra:
                    return FlatExtra;
            }
            return 0.0f;
        }

        protected virtual void AddCurrent(float currentValue)
        {
            this.m_Current += currentValue;
        }

        protected virtual void AddFlat(float flatValue)
        {
            this.m_Flat += flatValue;
        }
        protected virtual void AddIncreased(float increasedValue)
        {
            this.m_Increased += increasedValue;
        }
        protected virtual void AddMore(float moreValue)
        {
            this.m_More *= moreValue;
        }
        protected virtual void AddFlatExtra(float extraValue)
        {
            this.m_FlatExtra += extraValue;
        }
        public static Stat operator +(Stat s1, Stat s2)
        {
            //s1.AddFlat(s2.Flat);
            //s1.AddIncreased(s2.Increased - 1.0f);
            //s1.AddMore(s2.More);
            //s1.AddFlatExtra(s2.FlatExtra);
            foreach(StatModifier statModifier in s2.m_StatModifiers)
            {
                s1.AddStatModifier(statModifier);
            }
            s1.m_Current += s2.m_Current;
            return s1;
        }

        public Stat Copy()
        {
            Stat copy = new Stat(this.ModType);
            Recalculate();
            copy.AddStatModifier(new StatModifier(m_Flat, CalculationType.Flat, this));
            copy.AddStatModifier(new StatModifier(m_FlatExtra, CalculationType.FlatExtra, this));
            copy.AddStatModifier(new StatModifier(m_More-1.0f, CalculationType.More, this));
            copy.AddStatModifier(new StatModifier(m_Increased-1.0f, CalculationType.Increased, this));
            //foreach (StatModifier statModifier in m_StatModifiers)
            //{
            //    copy.AddStatModifier(statModifier);
            //
            

            copy.m_Percentage = m_Percentage;
            copy.m_Current = m_Current;
            copy.Recalculate();

            return copy;
        }

        public override bool Equals(object obj)
        {
            var stat = obj as Stat;
            return stat != null &&
                   m_Flat == stat.m_Flat &&
                   m_Increased == stat.m_Increased &&
                   m_More == stat.m_More &&
                   m_FlatExtra == stat.m_FlatExtra &&
                   m_Calculated == stat.m_Calculated &&
                   m_CalculatedNoExtra == stat.m_CalculatedNoExtra &&
                   m_Current == stat.m_Current;
        }


        #endregion
    }
}
