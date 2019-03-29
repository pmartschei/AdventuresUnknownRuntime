using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Enemies
{
    public class EnemyModel : LocalizedMonoBehaviour
    {
        private Enemy m_Enemy;

        #region Properties
        public Enemy Enemy { get => m_Enemy; set => m_Enemy = value; }
        #endregion

        #region Methods
        public override void OnLanguageChange()
        {

        }
        #endregion
    }
}
