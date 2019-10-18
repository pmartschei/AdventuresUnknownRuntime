using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu]
    public class MinionTargetCommandState : EntityState
    {

        [SerializeField] private float m_RetargetTime = 1.0f;

        private float m_CurrentTime = 0.0f;
        public override void OnEnter()
        {
            base.OnEnter();
            m_CurrentTime = 0.0f;
        }
        public override void Update()
        {
            base.Update();

            m_CurrentTime -= Time.deltaTime;

            if (m_CurrentTime <= 0.0f)
            {
                m_CurrentTime = m_RetargetTime;
                Entity entity = CommonComponents.EntityController.Entity;
                List<Entity> minions = entity.GetAllMinions();

                foreach(Entity minion in minions)
                {
                    minion.EntityBehaviour.EntityController.SwitchTarget(CommonComponents.EntityController.Target);
                }
            }
        }
    }
}
