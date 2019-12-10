using AdventuresUnknownSDK.Core.UI.Interfaces;
using AdventuresUnknownSDK.Core.Utils.UnityEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI
{
    public class UIFormatText : IGameText
    {

        [SerializeField] private TMP_Text m_Text = null;
        [SerializeField] private StringEvent m_OnTextChange = null;
        [SerializeField] private string m_Format = "";


        #region Properties

        protected TMP_Text Text { get => m_Text; set => m_Text = value; }
        protected StringEvent OnTextChange { get => m_OnTextChange; set => m_OnTextChange = value; }

        #endregion

        #region Methods

        #endregion
        public override void SetText(object obj)
        {
            if (!m_Text) return;
            string text = Formatize(obj);
            if (!m_Text.text.Equals(text))
            {
                m_Text.text = text;
                m_OnTextChange.Invoke(text);
            }
        }
        public override void SetColor(Color color)
        {
            if (!m_Text) return;
            m_Text.color = color;
        }

        protected virtual string Formatize(object obj)
        {
            try
            {
                return String.Format(m_Format, obj);
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            return obj.ToString();
        }
    }
}
