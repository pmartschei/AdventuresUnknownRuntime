using AdventuresUnknownSDK.Core.Attributes;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Utils.Events;
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

        private StatEvent m_FlatEvent;
        private StatEvent m_IncreasedEvent;
        private StatEvent m_MoreEvent;
        private StatEvent m_FlatExtraEvent;
        private StatEvent m_CurrentEvent;
        private StatEvent m_CalculatedEvent;
        private StatEvent m_PercentageEvent;

        #region Properties
        public string ModTypeIdentifier { get => m_ModType == null ? "" : m_ModType.Identifier; }
        public ModType ModType { get => m_ModType; }

        public float Increased
        {
            get
            {
                Rebuild();
                return m_Increased;
            }
            private set
            {
                if (value != m_Increased)
                {
                    m_Increased = value;
                    m_IncreasedEvent?.Invoke(this);
                }
            }
        }
        public float More
        {
            get
            {
                Rebuild();
                return m_More;
            }
            private set
            {
                if (value != m_More)
                {
                    m_More = value;
                    m_MoreEvent?.Invoke(this);
                }
            }
        }
        public float Flat
        {
            get
            {
                Rebuild();
                return m_Flat;
            }

            private set
            {
                if (value != m_Flat)
                {
                    m_Flat = value;
                    m_FlatEvent?.Invoke(this);
                }
            }
        }
        public float FlatExtra
        {
            get
            {
                Rebuild();
                return m_FlatExtra;
            }
            private set
            {
                if (value != m_FlatExtra)
                {
                    m_FlatExtra = value;
                    m_FlatExtraEvent?.Invoke(this);
                }
            }
        }
        public float Percentage
        {
            get
            {
                Rebuild();
                return m_Percentage;
            }
            private set
            {
                if (value != m_Percentage)
                {
                    m_Percentage = value;
                    m_PercentageEvent?.Invoke(this);
                }
            }
        }
        public float Calculated
        {
            get
            {
                Rebuild();
                return m_Calculated;
            }
        }
        public float CalculatedNoExtra
        {
            get
            {
                Rebuild();
                return m_CalculatedNoExtra;
            }
        }
        public float Current
        {
            get
            {
                Rebuild();
                return m_Current;
            }
            set
            {
                Rebuild();
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
                    m_CurrentEvent?.Invoke(this);
                    if (m_Calculated != 0.0f)
                    {
                        Percentage = m_Current / m_Calculated;
                    }
                    else
                    {
                        Percentage = 0.0f;
                    }
                    //StatChanged = true;
                }
            }
        }

        //public bool StatChanged { get; set; }
        public bool IsDirty
        {
            get
            {
                if (m_IsDirty) return true;
                foreach (StatModifier statModifier in m_StatModifiers)
                {
                    if (statModifier.IsDirty) return true;
                }
                return false;
            }
            set
            {
                m_IsDirty = value;
                foreach (StatModifier statModifier in m_StatModifiers)
                {
                    statModifier.IsDirty = value;
                }
            }
        }

        public bool IsDefault
        {
            get
            {
                if (Flat != 0.0f ||
                    Increased != 1.0f ||
                    More != 1.0f ||
                    FlatExtra != 0.0f ||
                    Calculated != 0.0f ||
                    CalculatedNoExtra != 0.0f) return false;
                return true;
            }
        }

        public event StatEvent OnFlatChange { add => m_FlatEvent += value; remove => m_FlatEvent -= value; }
        public event StatEvent OnIncreasedChange { add => m_IncreasedEvent += value; remove => m_IncreasedEvent -= value; }
        public event StatEvent OnMoreChange { add => m_MoreEvent += value; remove => m_MoreEvent -= value; }
        public event StatEvent OnFlatExtraChange { add => m_FlatExtraEvent += value; remove => m_FlatExtraEvent -= value; }
        public event StatEvent OnCurrentChange { add => m_CurrentEvent += value; remove => m_CurrentEvent -= value; }
        public event StatEvent OnCalculatedChange { add => m_CalculatedEvent += value; remove => m_CalculatedEvent -= value; }
        public event StatEvent OnPercentageChange { add => m_PercentageEvent += value; remove => m_PercentageEvent -= value; }
        #endregion

        #region Methods
        public void AddStatModifier(StatModifier statModifier)
        {
            if (m_StatModifiers.Contains(statModifier)) return;
            m_StatModifiers.Add(statModifier);
            statModifier.OnIsDirty += OnStatModifierDirty;
            m_IsDirty = true;
            OnStatModifierDirty();
            //StatChanged = true;
        }

        private void OnStatModifierDirty()
        {
            if (m_FlatEvent?.GetInvocationList().Length > 0 ||
                m_IncreasedEvent?.GetInvocationList().Length > 0 ||
                m_MoreEvent?.GetInvocationList().Length > 0 ||
                m_FlatExtraEvent?.GetInvocationList().Length > 0 ||
                m_CurrentEvent?.GetInvocationList().Length > 0 ||
                m_CalculatedEvent?.GetInvocationList().Length > 0 ||
                m_PercentageEvent?.GetInvocationList().Length > 0)
            {
                Rebuild();
            }
        }

        private void Rebuild()
        {
            if (IsDirty)
            {
                float percentage = m_Percentage;

                RebuildValues();

                Recalculate();

                IsDirty = false;
                Current = Calculated * percentage;
            }
        }

        private void RebuildValues()
        {
            float flat = 0.0f;
            float increased = 1.0f;
            float more = 1.0f;
            float flatExtra = 0.0f;

            foreach (StatModifier sm in m_StatModifiers)
            {
                switch (sm.CalculationType)
                {
                    case CalculationType.Flat:
                        flat = AddValue(flat, sm.Value, sm.CalculationType);
                        break;
                    case CalculationType.Increased:
                        increased = AddValue(increased, sm.Value, sm.CalculationType);
                        break;
                    case CalculationType.More:
                        more = AddValue(more, sm.Value, sm.CalculationType);
                        break;
                    case CalculationType.FlatExtra:
                        flatExtra = AddValue(flatExtra, sm.Value, sm.CalculationType);
                        break;
                }
            }

            Flat = flat;
            Increased = increased;
            More = more;
            FlatExtra = flatExtra;
        }

        public bool RemoveStatModifier(StatModifier statModifier)
        {
            bool removed = m_StatModifiers.Remove(statModifier);
            if (removed)
            {
                m_IsDirty = true;
                statModifier.OnIsDirty -= OnStatModifierDirty;
                //StatChanged = true;
            }
            return removed;
        }
        public void RemoveStatModifiersBySource(object source)
        {
            StatModifier[] statModifiers = GetStatModifiersBySource(source);
            foreach (StatModifier statModifier in statModifiers)
            {
                RemoveStatModifier(statModifier);
            }
        }
        public StatModifier[] GetStatModifiersBySource(object source)
        {
            List<StatModifier> statModifiers = new List<StatModifier>();
            foreach (StatModifier statModifier in m_StatModifiers)
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
            Reset();
            Recalculate();
        }
        protected virtual void Reset()
        {
            m_Flat = 0.0f;
            m_Increased = 1.0f;
            m_More = 1.0f;
            m_FlatExtra = 0.0f;
            m_Calculated = 0.0f;
            m_CalculatedNoExtra = 0.0f;
        }

        protected virtual void Recalculate()
        {
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
                //StatChanged = true;
            }
            newCalculated += m_FlatExtra;
            if (roundDown)
                newCalculated = Mathf.Floor(newCalculated);
            if (newCalculated != m_Calculated)
            {
                m_Calculated = newCalculated;
                //StatChanged = true;
                m_CalculatedEvent?.Invoke(this);
            }
            float newCurrentValue = m_Current;
            if (alwaysTakeMax)
            {
                newCurrentValue = m_Calculated;
            }
            else
            {
                if (canGetHigherThanCalc)
                {
                    newCurrentValue = Mathf.Clamp(newCurrentValue, minValue, maxValue);
                }
                else
                {
                    newCurrentValue = Mathf.Clamp(newCurrentValue, minValue, m_Calculated);
                }
            }
            if (m_Current != newCurrentValue)
            {
                m_Current = newCurrentValue;
                m_CurrentEvent?.Invoke(this);
            }
            if (m_Calculated != 0.0f)
            {
                Percentage = m_Current / m_Calculated;
            }
            else
            {
                Percentage = 0.0f;
            }
        }

        private float AddValue(float value, float addition, CalculationType calculationType)
        {
            switch (calculationType)
            {
                //Technically, could be solved via default
                case CalculationType.Flat:
                case CalculationType.FlatExtra:
                case CalculationType.Increased:
                    return value + addition;
                case CalculationType.More:
                    return value * (addition + 1.0f);
                default:
                    return value + addition;
            }
        }

        public float GetValue(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.Flat:
                    return Flat;
                case CalculationType.Increased:
                    return Increased - 1.0f;
                case CalculationType.More:
                    return More - 1.0f;
                case CalculationType.Calculated:
                    return Calculated;
                case CalculationType.FlatExtra:
                    return FlatExtra;
                case CalculationType.Percentage:
                    return Percentage;
            }
            return 0.0f;
        }

        public static Stat operator +(Stat s1, Stat s2)
        {
            foreach (StatModifier statModifier in s2.m_StatModifiers)
            {
                s1.AddStatModifier(statModifier);
            }
            s1.m_Current += s2.m_Current;
            return s1;
        }

        public Stat Copy()
        {
            Stat copy = new Stat(this.ModType);

            foreach(StatModifier statModifier in m_StatModifiers)
            {
                copy.AddStatModifier(new StatModifier(statModifier.Value,statModifier.CalculationType,statModifier.Source));
            }
            //copy.AddStatModifier(new StatModifier(m_Flat, CalculationType.Flat, this));
            //copy.AddStatModifier(new StatModifier(m_FlatExtra, CalculationType.FlatExtra, this));
            //copy.AddStatModifier(new StatModifier(m_More - 1.0f, CalculationType.More, this));
            //copy.AddStatModifier(new StatModifier(m_Increased - 1.0f, CalculationType.Increased, this));

            copy.m_Percentage = m_Percentage;
            copy.m_Current = m_Current;
            copy.Rebuild();

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

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(ModTypeIdentifier);
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
        #endregion
    }
}
