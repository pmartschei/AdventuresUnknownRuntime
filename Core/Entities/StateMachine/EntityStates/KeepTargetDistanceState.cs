using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EntityStates/KeepTargetDistanceState", fileName = "KeepTargetDistanceState.asset")]
    public class KeepTargetDistanceState : EntityState
    {
        [SerializeField] private float m_KeepTargetDistance = 5.0f;
        private float m_SqrKeepTargetDistance = 0.0f;

        #region Properties
        #endregion

        #region Methods
        public override void OnEnter()
        {
            base.OnEnter();
            m_SqrKeepTargetDistance = m_KeepTargetDistance * m_KeepTargetDistance;
        }
        public override void Update()
        {
            base.Update();

            if (CommonComponents.EntityController.Target == null)
            {
                EntityStateMachine.SetNextStateToMain();
                return;
            }

            float distance = Vector3.SqrMagnitude(gameObject.transform.position - CommonComponents.EntityController.Target.transform.position);
            if (distance > m_SqrKeepTargetDistance)
            {
                CommonComponents.EntityController.Target = null;
                EntityStateMachine.SetNextStateToMain();
                return;
            }
        }
        public override void OnDrawGizmosSelected()
        {
            if (CommonComponents.EntityController.Target == null) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(CommonComponents.EntityController.Target.transform.position, 0.5f);
        }
        #endregion
    }
}
