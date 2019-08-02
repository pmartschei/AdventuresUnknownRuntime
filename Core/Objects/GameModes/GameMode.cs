using AdventuresUnknownSDK.Core.Objects.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.GameModes
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/GameModes/GameMode", fileName="GameMode.asset")]
    public class GameMode : CoreObject
    {
        [SerializeField] private LocalizationString m_GameModeName = null;
        [SerializeField] private Sprite m_Icon = null;
        [SerializeField] private string m_FolderName = "";

        #region Properties
        public Sprite Icon { get => m_Icon; set => m_Icon = value; }
        public string GameModeName { get => m_GameModeName.LocalizedString; }
        public string FolderName { get => m_FolderName; set => m_FolderName = value; }
        #endregion

        #region Methods
        public override void ForceUpdate()
        {
            base.ForceUpdate();
            m_GameModeName.ForceUpdate();
        }
        #endregion
    }
}
