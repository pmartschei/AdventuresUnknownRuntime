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
        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            Stat mod = activeStats.GetStat(ModType.Identifier);
            mod.OnCurrentChange += new InternalCheck(activeStats).CheckMinions;
        }

        private class InternalCheck
        {
            private Entity m_Entity;
            public InternalCheck(Entity entity)
            {
                m_Entity = entity;
            }
            public void CheckMinions(Stat stat)
            {
                int minionsToDelete = (int)(stat.Current - stat.Calculated);
                if (minionsToDelete > 0)
                {
                    List<Entity> minions = m_Entity.GetMinions(stat.ModTypeIdentifier);

                    for (int i = 0; i < minionsToDelete; i++)
                    {
                        minions[i].Notify(ActionTypeManager.Death);
                    }
                }
            }
        }
        #endregion
    }
}
