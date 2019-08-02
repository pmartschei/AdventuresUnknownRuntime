using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Logic.ActiveGemContainers;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Enemies;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using Panda;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(EnemyActiveGemContainer))]
    [RequireComponent(typeof(Animator))]
    public class EnemyController : EntityController
    {
        [SerializeField] private EntityBehaviour m_EntityComponent = null;
        
        private EnemyActiveGemContainer m_EnemyActiveGemContainer = null;


        #region Properties
        public Rigidbody RigidBody { get; private set; }
        #endregion


        #region Methods
        private void Awake()
        {
            Animator = GetComponent<Animator>();
            m_EnemyActiveGemContainer = GetComponent<EnemyActiveGemContainer>();
            RigidBody = GetComponent<Rigidbody>();
            SpaceShip = m_EntityComponent;
            LookingDestination = transform.position + new Vector3(0, 1, 0);
        }

        private void Update()
        {
            if (SpaceShip.Entity.IsDead)
            {
                Destroy(this.gameObject);
            }
        }

        private void FixedUpdate()
        {
            RigidBody.drag = SpaceShip.Entity.GetStat("core.modtypes.ship.movementresistance").Calculated;
            Stat speed = SpaceShip.Entity.GetStat("core.modtypes.ship.movementspeed");
            //repel
            Collider[] colliders = Physics.OverlapSphere(this.transform.position, 0.75f);

            foreach(Collider collider in colliders)
            {
                EntityController controller = collider.GetComponentInParent<EntityController>();
                if (controller == null) continue;
                if (controller.SpaceShip.Entity.Description.Enemy != this.SpaceShip.Entity.Description.Enemy) continue;
                RigidBody.AddForce((this.transform.position - controller.gameObject.transform.position).normalized * 10*speed.Calculated);
            }
            //repel

            if (RigidBody.velocity.magnitude > speed.Calculated)
            {
                RigidBody.velocity = RigidBody.velocity.normalized * speed.Calculated;
            }
            speed.Current = RigidBody.velocity.magnitude;
        }
        
        public void SetAnimFloat(string name,float value)
        {
            Animator.SetFloat(name, value);
        }
        public void SetAnimBool(string name, bool value)
        {
            Animator.SetBool(name, value);
        }
        public void SetAnimInt(string name, int value)
        {
            Animator.SetInteger(name, value);
        }

        public void SetAnimTrigger(string name)
        {
            Animator.SetTrigger(name);
        }
        
        public void Attack()
        {
            m_EnemyActiveGemContainer.Spawn(0, transform.position, LookingDestination);
        }
        
        public bool IsNearTarget()
        {
            Vector3 distance = transform.position - Target.transform.position;
            float attackMax = m_EnemyActiveGemContainer.GetNextAttackMaxDistance();
            attackMax *= attackMax;
            return distance.sqrMagnitude <= (attackMax * 0.9f);
        }
        public bool HasCooldown()
        {
            return CooldownManager.HasCooldown(SpaceShip.Entity);
        }
        
        public void AimTowardsTarget()
        {
            if (Target)
            {
                AimTowardsPosition(Target.transform.position);
            }
        }
        public bool AimTowardsPosition(Vector3 position)
        {
            float z = Mathf.Atan2((position.y - transform.position.y),
                (position.x - transform.position.x)) * Mathf.Rad2Deg;
            float distance = Vector3.Distance(transform.position, position);
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z - 90.0f);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 360.0f);
            LookingDestination = transform.up * distance + transform.position;
            Quaternion normTransform = Quaternion.Normalize(transform.rotation);
            Quaternion normTarget = Quaternion.Normalize(targetRotation);
            if (Quaternion.Angle(normTransform,normTarget) <= 0.01f) return true;
            return false;
        }

        public void RotateTowardsPosition(Vector3 position)
        {
            float z = Mathf.Atan2((position.y - transform.position.y),
                (position.x - transform.position.x)) * Mathf.Rad2Deg;
            float distance = Vector3.Distance(transform.position, position);
            Quaternion targetRotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, z - 90.0f);
            transform.rotation = targetRotation;
            LookingDestination = transform.up * distance + transform.position;
        }

        public void MoveTowardsTarget()
        {
            if (Target)
            {
                MoveTowardsPosition(Target.transform.position);
            }
        }

        public void MoveTowardsPosition(Vector3 position)
        {
            Vector3 distance = position - transform.position;
            Stat speed = SpaceShip.Entity.GetStat("core.modtypes.ship.acceleration");
            RigidBody.AddForce(distance.normalized * speed.Calculated);
        }

        #endregion
    }
}
