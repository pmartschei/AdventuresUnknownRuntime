using AdventuresUnknownSDK.Core.Objects.Mods;

namespace AdventuresUnknownSDK.Core.Entities
{
    public class StatModifier
    {
        private float m_Value = 0.0f;
        private CalculationType m_CalculationType = CalculationType.Flat;
        private object m_Source = null;
        private bool m_IsDirty = false;

        public delegate void VoidEvent();

        private VoidEvent m_IsDirtyEvent;

        public StatModifier(float value, CalculationType calculationType,object source)
        {
            this.m_Value = value;
            this.m_CalculationType = calculationType;
            this.m_Source = source;
        }

        public virtual CalculationType CalculationType {
            get => m_CalculationType;
            set
            {
                if (value != m_CalculationType)
                {
                    m_CalculationType = value;
                    m_IsDirty = true;
                    m_IsDirtyEvent?.Invoke();
                }
            }
        }
        public virtual float Value {
            get => m_Value;
            set
            {
                if (value != m_Value)
                {
                    m_Value = value;
                    m_IsDirty = true;
                    m_IsDirtyEvent?.Invoke();
                }
            }
        }
        public virtual bool IsDirty { get => m_IsDirty; set => m_IsDirty = value; }
        public virtual object Source { get => m_Source; set => m_Source = value; }
        public event VoidEvent OnIsDirty { add => m_IsDirtyEvent +=value; remove => m_IsDirtyEvent -= value; }
    }
}