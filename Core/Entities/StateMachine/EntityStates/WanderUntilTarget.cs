using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu]
    public class WanderUntilTarget : WanderState
    {
        [SerializeField] private EntityState m_NextState = null;
        public override void Update()
        {
            base.Update();
            if (CommonComponents.EntityController.Target && m_NextState)
            {
                if (CommonComponents.TranslationalController != null)
                    CommonComponents.TranslationalController.StopMove();
                EntityStateMachine.SetNextState(m_NextState);
            }
        }
    }
}
