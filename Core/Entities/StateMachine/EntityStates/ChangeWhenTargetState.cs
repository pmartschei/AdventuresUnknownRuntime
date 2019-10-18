using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu]
    public class ChangeWhenTargetState : EntityState
    {
        [SerializeField] private EntityState m_NextState = null;
        [SerializeField] private bool m_HasTarget = false;

        public override void Update()
        {
            base.Update();

            bool flag = (CommonComponents.EntityController.Target == m_HasTarget);
            if (flag)
            {
                if (m_NextState)
                {
                    EntityStateMachine.SetNextState(m_NextState);
                }
                else
                {
                    EntityStateMachine.SetNextStateToMain();
                }
            }
        }
    }
}
