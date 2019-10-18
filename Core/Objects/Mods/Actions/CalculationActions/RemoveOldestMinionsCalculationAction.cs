using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.CalculationActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Calculation/RemoveOldestMinionsCalculationAction", fileName = "RemoveOldestMinionsCalculationAction.asset")]
    public class RemoveOldestMinionsCalculationAction : CalculationAction
    {


        #region Properties

        #endregion

        #region Methods
        public override void Initialize(Entity activeStats)
        {
            base.Initialize(activeStats);
            Stat mod = activeStats.GetStat(ModType.Identifier);
            activeStats.NotifyOnStatChange(mod, this);
        }
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            Stat mod = activeStats.GetStat(ModType.Identifier);
            int minionsToDelete = (int)(mod.Current - mod.Calculated);
            if (minionsToDelete > 0)
            {
                List<Entity> minions = activeStats.GetMinions(ModType.Identifier);

                for(int i = 0; i < minionsToDelete; i++)
                {
                    minions[i].Notify(ActionTypeManager.Death);
                }
            }
        }
        #endregion
    }
}
