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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EntityStates/AttackPatternState", fileName = "AttackPatternState.asset")]
    public class AttackPatternState : EntityState
    {
        [SerializeField] private AttackState[] m_EntityStates = null;
        [SerializeField] private bool m_TargetSearchEnabled = false;
        [SerializeField] private bool m_DistanceCheckEnabled = false;
        [SerializeField] private float m_MaxTargetRadius = 25.0f;

        private AttackState[] m_InstancedEntityStates = null;
        private int m_CurrentState = 0;
        private List<EntityController> m_AvailableTargets = null;
        #region Properties
        public AttackState[] InstancedEntityStates { get => m_InstancedEntityStates; set => m_InstancedEntityStates = value; }
        public int CurrentState { get => m_CurrentState; }
        #endregion

        #region Methods
        public override void OnEnter()
        {
            base.OnEnter();

            m_InstancedEntityStates = new AttackState[m_EntityStates.Length];
            for (int i = 0; i < m_EntityStates.Length; i++)
            {
                m_InstancedEntityStates[i] = Instantiate(m_EntityStates[i]);
                m_InstancedEntityStates[i].EntityStateMachine = this.EntityStateMachine;
            }

            if (m_CurrentState < m_InstancedEntityStates.Length)
            {
                if (m_InstancedEntityStates[m_CurrentState] != null)
                    m_InstancedEntityStates[m_CurrentState].OnEnter();
            }
        }
        public override void OnExit()
        {
            base.OnExit();
            if (m_CurrentState < m_InstancedEntityStates.Length) { 
                if (m_InstancedEntityStates[m_CurrentState] != null)
                    m_InstancedEntityStates[m_CurrentState].OnExit();
            }
        }
        public override void Update()
        {
            base.Update();

            if (m_TargetSearchEnabled)
            {
                SearchTarget();
            }

            if (!CheckDistance()) return;
            if (m_CurrentState < m_InstancedEntityStates.Length)
            {
                if (m_InstancedEntityStates[m_CurrentState] != null)
                    m_InstancedEntityStates[m_CurrentState].Update();
            }
        }


        public override void FixedUpdate()
        {
            base.FixedUpdate();
            if (!CheckDistance()) ;
            if (m_CurrentState < m_InstancedEntityStates.Length)
            {
                if (m_InstancedEntityStates[m_CurrentState] != null)
                {
                    m_InstancedEntityStates[m_CurrentState].FixedUpdate();
                    if (m_InstancedEntityStates[m_CurrentState].IsFinished)
                    {
                        m_InstancedEntityStates[m_CurrentState].OnExit();
                        do
                        {
                            m_CurrentState++;
                            if (m_CurrentState >= m_InstancedEntityStates.Length)
                            {
                                m_CurrentState = 0;
                            }
                        } while (m_InstancedEntityStates[m_CurrentState] == null);
                        m_InstancedEntityStates[m_CurrentState].OnEnter();
                    }
                }
            }
        }
        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
            if (m_CurrentState < m_InstancedEntityStates.Length)
            {
                if (m_InstancedEntityStates[m_CurrentState] != null)
                    m_InstancedEntityStates[m_CurrentState].OnDrawGizmos();
            }
        }
        public override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            if (m_TargetSearchEnabled)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(this.gameObject.transform.position, m_MaxTargetRadius);
                if (CommonComponents.EntityController.Target != null)
                    Gizmos.DrawWireSphere(CommonComponents.EntityController.Target.transform.position, 0.5f);
            }
            if (m_CurrentState < m_InstancedEntityStates.Length)
            {
                if (m_InstancedEntityStates[m_CurrentState] != null)
                    m_InstancedEntityStates[m_CurrentState].OnDrawGizmosSelected();
            }
        }

        protected virtual bool CheckDistance()
        {
            if (!m_DistanceCheckEnabled) return true;
            if (CommonComponents.AttackController == null) return true;
            float maxRange = -1.0f;

            foreach (AttackState attackState in m_InstancedEntityStates)
            {
                if (attackState == null) continue;
                if (!CommonComponents.AttackController.RequiresTarget(attackState.SkillIndex))
                {
                    maxRange = float.MaxValue;
                    continue;
                }
                if (CommonComponents.EntityController.Target == null) continue;
                maxRange = Mathf.Max(CommonComponents.AttackController.GetAttackMaxDistance(attackState.SkillIndex),maxRange);
            }

            if (CommonComponents.EntityController.Target == null) return maxRange == -1.0f;
            float distance = Vector3.SqrMagnitude(CommonComponents.EntityController.Head.position - CommonComponents.EntityController.Target.Head.position);
            if (distance > maxRange * maxRange || distance > m_MaxTargetRadius * m_MaxTargetRadius)
            {
                CommonComponents.EntityController.Target = null;
                return false;
            }
            return true;
        }
        protected virtual EntityController FindBestTarget(List<EntityController> targets)
        {
            if (targets.Count == 0) return null;
            return targets[0];
        }
        protected virtual void SearchTarget()
        {
            Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, m_MaxTargetRadius, ~(1 << this.gameObject.layer));
            m_AvailableTargets = new List<EntityController>();

            foreach (Collider collider in colliders)
            {
                if (Physics.GetIgnoreLayerCollision(this.gameObject.layer, collider.gameObject.layer)) continue;
                EntityController entityController = collider.gameObject.GetComponentInParent<EntityController>();
                if (!entityController) continue;
                if (entityController.Entity.Description.EntityType != EntityType.SpaceShip) continue;
                m_AvailableTargets.Add(entityController);
            }
            CommonComponents.EntityController.Target = FindBestTarget(m_AvailableTargets);
        }
        #endregion
    }
}
