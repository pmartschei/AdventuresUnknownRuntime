using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates.Muzzles
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EntityStates/Muzzles/MuzzleKeepTargetDistanceState", fileName = "MuzzleKeepTargetDistanceState.asset")]
    public class MuzzleKeepTargetDistanceState : KeepTargetDistanceState
    {
        #region Properties
        #endregion

        #region Methods
        public override void Update()
        {
            base.Update();

            MuzzleController muzzleController = CommonComponents.EntityController as MuzzleController;
            if (!muzzleController || muzzleController.Target == null) return;
            if (!muzzleController.IsValidTarget(muzzleController.Target.transform.position))
            {
                muzzleController.Target = null;
                EntityStateMachine.SetNextStateToMain();
            }
        }
        #endregion
    }
}
