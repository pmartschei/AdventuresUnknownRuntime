using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces;
using AdventuresUnknownSDK.Core.Entities.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EntityStates/MoveTowardsTargetState", fileName = "MoveTowardsTargetState.asset")]
    public class MoveTowardsTargetState : EntityState
    {


        #region Properties

        #endregion

        #region Methods
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public override void Update()
        {
            base.Update();
            if (CommonComponents.TranslationalController == null) return;

            if (!CommonComponents.TranslationalController.MoveTowardsTarget())
            {
                CommonComponents.TranslationalController.StopMove();
            }
        }
        #endregion
    }
}
