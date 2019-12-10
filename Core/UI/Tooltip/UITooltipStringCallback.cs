using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Tooltip
{
    
    public class UITooltipStringCallback : MonoBehaviour
    {
        [SerializeField] private UITooltip m_UiTooltip = null;
        [SerializeField] private string m_Value = null;

        public string Value
        {
            get => m_Value;
            set
            {
                if (m_Value != value)
                {
                    m_Value = value;
                    m_UiTooltip?.UpdateDisplay();
                }
            }
        }

        private void OnEnable()
        {
            if (m_UiTooltip)
            {
                m_UiTooltip.Callback = GetValueCallback;
            }
        }
        protected string GetValueCallback()
        {
            return Value;
        }

        public void OnValidate()
        {
            m_UiTooltip?.UpdateDisplay();
        }

    }
}
