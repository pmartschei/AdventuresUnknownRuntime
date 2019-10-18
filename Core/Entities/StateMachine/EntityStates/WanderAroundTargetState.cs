using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu]
    public class WanderAroundTargetState : WanderState
    {
        [SerializeField] private EntityState m_NoTargetFoundState = null;

        protected override bool Wander()
        {
            if (!CommonComponents.EntityController.Target)
            {
                if (m_NoTargetFoundState)
                {
                    EntityStateMachine.SetNextState(m_NoTargetFoundState);
                }
                return false;
            }

            Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * MaxMoveRadius;

            randomPosition += CommonComponents.EntityController.Target.Head.position;

            return CommonComponents.TranslationalController.MoveTowardsPosition(randomPosition);
        }
    }
}
