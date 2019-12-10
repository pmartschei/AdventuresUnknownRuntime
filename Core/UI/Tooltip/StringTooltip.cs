using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AdventuresUnknownSDK.Core.UI.Tooltip
{
    public class StringTooltip : Tooltip<string>
    {
        [SerializeField] private TMP_Text m_Text = null;
        [SerializeField] private LayoutGroup m_LayoutGroup = null;
        public override void Display(string t)
        {
            if (m_Text)
            {
                m_Text.text = t;
            }
        }
        public override void Display(object obj)
        {
            if (obj != null)
            {
                Display(obj.ToString());
            }
            else
            {
                Display("null");
            }
        }
        public override void Anchor(TextAnchor anchor)
        {
            if (m_LayoutGroup)
            {
                m_LayoutGroup.childAlignment = anchor;
            }
        }
    }
}
