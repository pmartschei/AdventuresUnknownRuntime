using System;
using AdventuresUnknownSDK.Core.Objects.Mods;

namespace AdventuresUnknownSDK.Core.Entities
{
    public class StatModifierByStat : StatModifier
    {
        private Stat m_Stat = null;
        private CalculationType? m_StatCalculationType;
        private bool m_IsListening = false;
        private Func<float, float> m_Action;

        public bool IsListening { get => m_IsListening;}
        public Stat Stat { get => m_Stat; }


        public StatModifierByStat(Stat stat, CalculationType calculationType, object source, Func<float,float> action = null) : base(0.0f, calculationType, source)
        {
            m_Action = action;
            m_Stat = stat;
            OnValueChange(m_Stat);
            StartListen();
        }
        public StatModifierByStat(Stat stat, CalculationType calculationType, object source, CalculationType statCalculationType, Func<float, float> action = null) : base(0.0f, calculationType, source)
        {
            m_Action = action;
            m_Stat = stat;
            m_StatCalculationType = statCalculationType;
            OnValueChange(m_Stat);
            StartListen();
        }

        public void StartListen()
        {
            if (m_IsListening) return;
            m_IsListening = true;
            switch (m_StatCalculationType.HasValue ? m_StatCalculationType.Value : CalculationType)
            {
                case CalculationType.Flat:
                    m_Stat.OnFlatChange += OnValueChange;
                    break;
                case CalculationType.Increased:
                    m_Stat.OnIncreasedChange += OnValueChange;
                    break;
                case CalculationType.More:
                    m_Stat.OnMoreChange += OnValueChange;
                    break;
                case CalculationType.FlatExtra:
                    m_Stat.OnFlatExtraChange += OnValueChange;
                    break;
                case CalculationType.Calculated:
                    m_Stat.OnCalculatedChange += OnValueChange;
                    break;
                case CalculationType.Percentage:
                    m_Stat.OnPercentageChange += OnValueChange;
                    break;
            }
            OnValueChange(m_Stat);
        }

        private void OnValueChange(Stat stat)
        {
            IsDirty = true;
        }

        public void StopListen()
        {
            if (!m_IsListening) return;
            m_IsListening = false;
            switch (m_StatCalculationType.HasValue ? m_StatCalculationType.Value : CalculationType)
            {
                case CalculationType.Flat:
                    m_Stat.OnFlatChange -= OnValueChange;
                    break;
                case CalculationType.Increased:
                    m_Stat.OnIncreasedChange -= OnValueChange;
                    break;
                case CalculationType.More:
                    m_Stat.OnMoreChange -= OnValueChange;
                    break;
                case CalculationType.FlatExtra:
                    m_Stat.OnFlatExtraChange -= OnValueChange;
                    break;
                case CalculationType.Calculated:
                    m_Stat.OnCalculatedChange -= OnValueChange;
                    break;
                case CalculationType.Percentage:
                    m_Stat.OnPercentageChange -= OnValueChange;
                    break;
            }
        }

        public override CalculationType CalculationType
        {
            get => base.CalculationType;
            set
            {
                if (value != base.CalculationType)
                {
                    bool isListening = m_IsListening;
                    if (isListening)
                    {
                        StopListen();
                    }
                    base.CalculationType = value;
                    if (isListening)
                    {
                        StartListen();
                    }
                }
            }
        }
        public override float Value
        {
            get
            {
                float value = m_Stat.GetValue(m_StatCalculationType.HasValue ? m_StatCalculationType.Value : CalculationType);
                if (m_Action != null)
                {
                    value = m_Action.Invoke(value);
                }
                return value;
            }
        }

        public override bool IsDirty
        {
            get
            {
                return base.IsDirty || m_Stat.IsDirty;
            }
            set
            {
                base.IsDirty = value;
                m_Stat.IsDirty = value;
            }
        }
    }
}