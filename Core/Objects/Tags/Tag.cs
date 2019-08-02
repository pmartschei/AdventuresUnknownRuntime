using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Tags
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Tags/Tag", fileName = "Tag.asset")]
    public class Tag : CoreObject
    {

        [SerializeField] private LocalizationString m_TagName = null;
        [SerializeField] private Color m_Color = Color.white;

        private string m_HTMLColor = "";


        #region Properties
        public string TagName { get => m_TagName.LocalizedString; }
        public Color Color { get => m_Color;
            set
            {
                if (m_Color != value)
                {
                    m_Color = value;
                    m_HTMLColor = ColorUtility.ToHtmlStringRGBA(m_Color);
                }
            }
        }
        public string HTMLColor { get => m_HTMLColor; }
        #endregion

        #region Methods
        public override void Initialize()
        {
            base.Initialize();
            m_HTMLColor = ColorUtility.ToHtmlStringRGBA(m_Color);
        }
        public override void ForceUpdate()
        {
            base.ForceUpdate();
            m_TagName.ForceUpdate();
        }
        #endregion
    }
}
