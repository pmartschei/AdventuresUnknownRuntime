using AdventuresUnknownSDK.Core.Objects.Mods;

namespace AdventuresUnknownSDK.Core.Entities
{
    public class StatModifier
    {
        private float m_Value = 0.0f;
        private CalculationType m_CalculationType = CalculationType.Flat;
        private object m_Source = null;
        private bool m_IsDirty = false;

        public StatModifier(float value, CalculationType calculationType,object source)
        {
            this.m_Value = value;
            this.m_CalculationType = calculationType;
            this.m_Source = source;
        }

        public CalculationType CalculationType {
            get => m_CalculationType;
            set
            {
                if (value != m_CalculationType)
                {
                    m_CalculationType = value;
                    m_IsDirty = true;
                }
            }
        }
        public float Value {
            get => m_Value;
            set
            {
                if (value != m_Value)
                {
                    m_Value = value;
                    m_IsDirty = true;
                }
            }
        }
        public bool IsDirty { get => m_IsDirty; set => m_IsDirty = value; }
        public object Source { get => m_Source; set => m_Source = value; }
    }
}