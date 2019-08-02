using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ConditionActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/ConditionActions/RollConditionAction", fileName = "RollConditionAction.asset")]
    public class RollConditionAction : ConditionAction
    {
        [SerializeField] private float m_MinRoll = 0.0f;
        [SerializeField] private float m_MaxRoll = 1.0f;
        #region Properties

        #endregion

        #region Methods

        #endregion
        public override bool Notify(Entity activeStats, ActionContext actionContext)
        {
            bool flag = false;

            Stat stat = activeStats.GetStat(ModType.Identifier);
            float roll = UnityEngine.Random.Range(m_MinRoll, m_MaxRoll);

            flag = roll <= stat.Calculated;

            return flag;
        }
    }
}
