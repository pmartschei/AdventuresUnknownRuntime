using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/ModType", fileName = "ModType.asset")]
    public class ModType : CoreObject
    {
        [SerializeField] private int m_Priority = 0;
        [SerializeField] private bool m_RoundDown = false;
        [SerializeField] private bool m_AlwaysTakeMax = false;
        [SerializeField] private float m_MinValue = float.MinValue;
        [SerializeField] private float m_MaxValue = float.MaxValue;

        [SerializeField] private Color m_Color = Color.white;
        [SerializeField] private ModTypeFormatter[] m_ModTypeFormatters = null;

        //temporary
        [SerializeField] private CalculationAction m_CalculationAction = null;
        [SerializeField] private TickAction m_TickAction = null;


        #region Properties
        public int Priority
        {
            get { return m_Priority; }
            set { this.m_Priority = value; }
        }
        public bool RoundDown
        {
            get { return m_RoundDown; }
            set { this.m_RoundDown = value; }
        }
        public bool AlwaysTakeMax
        {
            get { return m_AlwaysTakeMax; }
            set { this.m_AlwaysTakeMax = value; }
        }
        public Color Color
        {
            get { return m_Color; }
            set { this.m_Color = value;
                HTMLColor = ColorUtility.ToHtmlStringRGBA(m_Color);
            }
        }
        public string HTMLColor { get; private set; }
        public float MinValue { get => m_MinValue; set => m_MinValue = value; }
        public float MaxValue { get => m_MaxValue; set => m_MaxValue = value; }
        #endregion


        #region Methods
        public void Calculate(IActiveStat iActiveStat)
        {
            if (!m_CalculationAction) return;
            m_CalculationAction.Calculate(iActiveStat, this);
        }
        public void OnTick(IActiveStat iActiveStat, float time)
        {
            if (!m_TickAction) return;
            m_TickAction.OnTick(iActiveStat, time, this);
        }
        public virtual string ToText(string formatterIdentifier, float value,CalculationType calculationType)
        {
            ModTypeFormatter modTypeFormatter = FindFirstFormatter(formatterIdentifier);
            return modTypeFormatter.ToText(value, this, calculationType, HTMLColor);
        }
        protected virtual ModTypeFormatter FindFirstFormatter(string identifier)
        {
            ModTypeFormatter foundFormatter = ModTypeFormatter.DefaultFormatter;
            if (m_ModTypeFormatters != null)
            {
                foreach(ModTypeFormatter formatter in m_ModTypeFormatters)
                {
                    if (formatter == null) continue;
                    if (formatter.Identifier.Equals(identifier))
                    {
                        foundFormatter = formatter;
                        break;
                    }
                }
            }
            return foundFormatter;
        }
        public void OnValidate()
        {
            HTMLColor = ColorUtility.ToHtmlStringRGBA(m_Color);
        }
        #endregion
    }
}
