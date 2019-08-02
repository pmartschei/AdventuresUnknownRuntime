using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.UtilityActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/RefreshStatAction", fileName = "RefreshStatAction.asset")]
    public class RefreshStatAction : MultipleBaseAction
    {
        #region Methods
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            Stat stat = activeStats.GetStat(ModType.Identifier);
            stat.Current = stat.Calculated;
        }
        #endregion

    }
}
