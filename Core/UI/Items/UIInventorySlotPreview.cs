using AdventuresUnknownSDK.Core.UI.Items.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace AdventuresUnknownSDK.Core.UI.Items
{
    public class UIInventorySlotPreview : IPreview
    {
        [SerializeField] private Image m_Image = null;
        [SerializeField] private Color m_ValidColor = Color.green;
        [SerializeField] private Color m_InvalidColor = Color.red;
        public override void Hide()
        {
            if (m_Image)
            {
                m_Image.enabled = false;
            }
        }

        public override void Show(bool valid)
        {
            if (m_Image)
            {
                m_Image.enabled = true;
                m_Image.color = valid ? m_ValidColor : m_InvalidColor;
            }
        }
    }
}
