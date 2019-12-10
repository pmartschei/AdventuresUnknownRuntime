using AdventuresUnknownSDK.Core.Objects.Mods;

namespace AdventuresUnknownSDK.Core.Entities
{
    public struct StatModifierStruct
    {
        private float m_Value;
        private CalculationType m_CalculationType;
        private object m_Source;
        private bool m_IsDirty;
        public CalculationType CalculationType
        {
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
        public float Value
        {
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
        public object Source { get => m_Source; set => m_Source = value; }
        public bool IsDirty { get => m_IsDirty; set => m_IsDirty = value; }
    }
}