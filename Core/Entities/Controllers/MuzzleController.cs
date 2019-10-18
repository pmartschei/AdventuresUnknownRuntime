using AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces;
using AdventuresUnknownSDK.Core.Entities.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.Controllers
{
    public class MuzzleController : EntityController,IAttackController, IRotationalController
    {
        [SerializeField] private EnemyController m_EnemyController = null;
        [SerializeField] private Muzzle m_Muzzle = null;
        #region Properties

        #endregion

        #region Methods
        public override void OnStart()
        {
            base.OnStart();
            this.SpaceShip = m_EnemyController.SpaceShip;
        }
        public bool IsValidTarget(Vector3 position)
        {
            float cone = Mathf.Cos(m_Muzzle.MaxRotation * Mathf.Deg2Rad);
            Transform transform = Head;
            Vector3 heading = (position - transform.position);
            heading.Normalize();
            return Vector3.Dot(Quaternion.Euler(0, m_Muzzle.DefaultRotation, 0) * m_Muzzle.transform.parent.forward, heading) > cone;
        }
        public bool AimTowardsTarget()
        {
            if (Target)
            {
                return AimTowardsPosition(Target.transform.position);
            }
            return false;
        }
        public bool AimTowardsDefault()
        {
            Transform transform = Head;
            Quaternion defaultRotation = Quaternion.LookRotation(Quaternion.Euler(0, m_Muzzle.DefaultRotation, 0) * m_Muzzle.transform.parent.forward);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, defaultRotation, Time.deltaTime * 180.0f);
            LookingDestination = transform.forward + transform.position;
            if (Quaternion.Angle(defaultRotation, transform.rotation) <= 0.01f) return true;
            return false;
        }
        public bool AimTowardsPosition(Vector3 position)
        {
            Transform transform = Head;
            float distance = Vector3.Distance(transform.position, position);
            position = position - transform.position;
            if (position == Vector3.zero) return true;
            Quaternion targetRotation = Quaternion.LookRotation(position);
            Quaternion defaultRotation = Quaternion.LookRotation(Quaternion.Euler(0, m_Muzzle.DefaultRotation, 0) * m_Muzzle.transform.parent.forward);
            targetRotation = Quaternion.RotateTowards(defaultRotation, targetRotation, m_Muzzle.MaxRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 180.0f);
            LookingDestination = transform.forward * distance + transform.position;
            //Quaternion normTransform = Quaternion.Normalize(transform.rotation);
            //Quaternion normTarget = Quaternion.Normalize(targetRotation);
            if (Quaternion.Angle(transform.rotation, targetRotation) <= 0.01f) return true;
            return false;
        }
        public void RotateTowardsPosition(Vector3 position)
        {
            Transform transform = Head;
            float distance = Vector3.Distance(transform.position, position);
            position = position - transform.position;
            if (position == Vector3.zero) return;
            Quaternion targetRotation = Quaternion.LookRotation(position);
            Quaternion defaultRotation = Quaternion.LookRotation(Quaternion.Euler(0, m_Muzzle.DefaultRotation, 0) * m_Muzzle.transform.parent.forward);
            targetRotation = Quaternion.RotateTowards(defaultRotation, targetRotation, m_Muzzle.MaxRotation);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * 180.0f);
            LookingDestination = transform.forward * distance + transform.position;
        }

        public void Attack(EntityController origin, int index, params Muzzle[] muzzles)
        {
            m_EnemyController.Attack(origin, index, m_Muzzle);
        }
        public void Attack(int index, params Muzzle[] muzzles)
        {
            m_EnemyController.Attack(this, index, m_Muzzle);
        }

        public bool IsNearTarget(int index, float multiplier = 0.9F)
        {
            Vector3 distance = transform.position - Target.transform.position;
            float attackMax = GetAttackMaxDistance(index);
            attackMax *= attackMax;
            return distance.sqrMagnitude <= (attackMax * multiplier);
        }

        public float GetAttackMaxDistance(int index)
        {
            return m_EnemyController.GetAttackMaxDistance(index);
        }

        public bool HasCooldown(int index)
        {
            return HasCooldown(this,index);
        }
        public bool HasCooldown(EntityController origin, int index)
        {
            return m_EnemyController.HasCooldown(origin,index);
        }

        public bool IsAttackValid(int index)
        {
            return m_EnemyController.IsAttackValid(index);
        }

        public bool RequiresTarget(int index)
        {
            return m_EnemyController.RequiresTarget(index);
        }

        public float GetAttackPriority(int index)
        {
            return m_EnemyController.GetAttackPriority(index);
        }

        public Entity GetEntity(int index)
        {
            return m_EnemyController.GetEntity(index);
        }

        #endregion
    }
}
