using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Localization;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Enemies
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EnemyType", fileName = "EnemyType.asset")]
    public class EnemyType : CoreObject
    {

        [SerializeField] private LocalizationString m_TypeName = null;


        #region Properties
        public string TypeName { get => m_TypeName.LocalizedString; }
        #endregion

        #region Methods
        public override void ForceUpdate()
        {
            base.ForceUpdate();
            m_TypeName.ForceUpdate();
        }
        #endregion
    }
}
