using AdventuresUnknownSDK.Core.Entities.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Enemies.Behaviours
{
    public abstract class EnemyStateMachineBehaviour : StateMachineBehaviour
    {
        private EnemyController m_EnemyController = null;


        #region Properties
        public EnemyController EnemyController => m_EnemyController;
        #endregion

        #region Methods
        public virtual void OnEnable() { }
        public virtual void OnUpdate() { }
        public virtual void OnDisable() { }
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            m_EnemyController = animator.gameObject.GetComponent<EnemyController>();
            OnEnable();
        }
        public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnUpdate();
        }
        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            OnDisable();
        }
        #endregion
    }
}
