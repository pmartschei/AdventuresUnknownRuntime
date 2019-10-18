using AdventuresUnknownSDK.Core.Entities.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates.Muzzles
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EntityStates/Muzzles/MuzzleSearchTargetState", fileName = "MuzzleSearchTargetState.asset")]
    public class MuzzleSearchTargetState : SearchTargetState
    {


        #region Properties

        #endregion

        #region Methods
        public override EntityController FindBestTarget(List<EntityController> targets)
        {
            MuzzleController muzzleController = CommonComponents.EntityController as MuzzleController;
            if (!muzzleController)
            {
                return base.FindBestTarget(targets);
            }
            else
            {
                foreach(EntityController target in targets)
                {
                    if (!muzzleController.IsValidTarget(target.transform.position)) continue;
                    return target;
                }
            }
            return null;
        }
        #endregion
    }
}
