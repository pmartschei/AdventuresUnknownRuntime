using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.UI.Interfaces;
using AdventuresUnknownSDK.Core.Utils.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.UI
{
    public class UIGameText : IGameText
    {
        [SerializeField] private TMP_Text m_Text = null;
        [SerializeField] private StringEvent m_OnTextChange = null;

        #region Properties

        protected TMP_Text Text { get => m_Text; set => m_Text = value; }
        protected StringEvent OnTextChange { get => m_OnTextChange; set => m_OnTextChange = value; }
        #endregion

        #region Methods
        public override void SetText(object obj)
        {
            if (!m_Text) return;
            string text = obj.ToString();
            if (!m_Text.text.Equals(text))
            {
                m_Text.text = text;
                m_OnTextChange.Invoke(text);
            }
        }
        #endregion
    }
}
