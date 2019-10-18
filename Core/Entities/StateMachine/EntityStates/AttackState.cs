using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces;
using AdventuresUnknownSDK.Core.Entities.StateMachine;
using AdventuresUnknownSDK.Core.Entities.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EntityStates/AttackState", fileName = "AttackState.asset")]
    public class AttackState : EntityState
    {
        [SerializeField] private int m_SkillIndex = 0;
        [SerializeField] private int m_ActivationCount = 0;
        [SerializeField] private float m_TimeCount = 0.0f;
        [SerializeField] private string[] m_Muzzles = null;

        protected float m_CurrentTime = 0.0f;
        protected int m_CurrentActivations = 0;

        #region Properties
        public bool IsFinished { get; set; }
        protected bool RequiresTarget { get; set; }
        public int SkillIndex { get => m_SkillIndex; set => m_SkillIndex = value; }
        public float TimeCount { get => m_TimeCount; set => m_TimeCount = value; }
        public int ActivationCount { get => m_ActivationCount; set => m_ActivationCount = value; }
        public string[] Muzzles { get => m_Muzzles; set => m_Muzzles = value; }
        #endregion

        #region Methods
        public override void OnEnter()
        {
            base.OnEnter();
            IsFinished = false;
            m_CurrentTime = 0.0f;
            m_CurrentActivations = 0;
            if (CommonComponents.AttackController == null || !CommonComponents.AttackController.IsAttackValid(SkillIndex))
            {
                IsFinished = true;
                return;
            }
            RequiresTarget = CommonComponents.AttackController.RequiresTarget(SkillIndex);
        }
        public override void Update()
        {
            base.Update();

            m_CurrentTime += Time.deltaTime;

            if (ActivationCount > 0 && m_CurrentActivations >= ActivationCount)
            {
                IsFinished = true;
            }
            if (TimeCount > 0.0f && m_CurrentTime >= TimeCount)
            {
                IsFinished = true;
            }

            if (IsFinished)
            {
                return;
            }

            if (PreAttack()) return;

            if (!RequiresTarget)
            {
                NoTargetAttack();
            }
            else
            {
                TargetAttack();
            }
        }

        public virtual float GetAIPriority()
        {
            return CommonComponents.AttackController.GetAttackPriority(SkillIndex);
        }

        protected virtual bool PreAttack()
        {
            if (CommonComponents.AttackController == null || !CommonComponents.AttackController.IsAttackValid(m_SkillIndex) || GetAIPriority() <= 0.0f)
            {
                IsFinished = true;
                return true;
            }
            if (RequiresTarget && CommonComponents.EntityController.Target == null)
            {
                return true;
            }
            return false;
        }

        protected virtual void NoTargetAttack()
        {
            DoAttack();
        }

        protected virtual void TargetAttack()
        {
            if (CommonComponents.AttackController.IsNearTarget(SkillIndex))
            {
                NearTarget();
            }
            else
            {
                NotNearTarget();
            }
        }

        protected virtual void NotNearTarget()
        {
            if (CommonComponents.TranslationalController != null)
                CommonComponents.TranslationalController.MoveTowardsTarget();
        }

        protected virtual void NearTarget()
        {
            if (CommonComponents.RotationalController.AimTowardsTarget())
            {
                DoAttack();
            }
        }

        protected virtual void DoAttack()
        {
            Muzzle[] muzzles = null;
            if (CommonComponents.MuzzleComponentController != null)
            {
                muzzles = CommonComponents.MuzzleComponentController.FindMuzzles(m_Muzzles);
            }
            if (!CommonComponents.AttackController.HasCooldown(SkillIndex))
            {
                CommonComponents.AttackController.Attack(SkillIndex,muzzles);
                m_CurrentActivations++;
            }
        }
        #endregion
    }
}
