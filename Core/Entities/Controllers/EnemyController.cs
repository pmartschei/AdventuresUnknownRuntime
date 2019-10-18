using AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces;
using AdventuresUnknownSDK.Core.Entities.Weapons;
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
using UnityEngine.AI;

namespace AdventuresUnknownSDK.Core.Entities.Controllers
{
    [RequireComponent(typeof(EnemyActiveGemContainer))]
    [RequireComponent(typeof(Animator))]
    //[RequireComponent(typeof(Rigidbody))]
    public class EnemyController : EntityController,IAttackController,ITranslationalController,IRotationalController, IMuzzleComponentController
    {
        [SerializeField] private EntityBehaviour m_EntityComponent = null;
        [SerializeField] private NavMeshAgent m_NavMeshAgent = null;
        [SerializeField] private Muzzle[] m_Muzzles = null;

        private Dictionary<string, Muzzle> m_MuzzleDictionary = new Dictionary<string, Muzzle>();

        private EnemyActiveGemContainer m_EnemyActiveGemContainer = null;


        #region Properties
        public Rigidbody RigidBody { get; private set; }
        public NavMeshAgent NavMeshAgent { get => m_NavMeshAgent; }
        public Muzzle[] Muzzles { get => m_Muzzles; set => m_Muzzles = value; }
        #endregion


        #region Methods
        public override void OnStart()
        {
            base.OnStart();

            foreach (Muzzle muzzle in m_Muzzles)
            {
                if (muzzle == null) continue;
                if (m_MuzzleDictionary.ContainsKey(muzzle.MuzzleName))
                {
                    GameConsole.LogErrorFormat("Skipped duplicate muzzle -> {0}, {1}", muzzle.MuzzleName, this.name);
                    continue;
                }
                m_MuzzleDictionary.Add(muzzle.MuzzleName, muzzle);
            }
        }
        private void Awake()
        {
            Animator = GetComponent<Animator>();
            m_EnemyActiveGemContainer = GetComponent<EnemyActiveGemContainer>();
            RigidBody = GetComponent<Rigidbody>();
            SpaceShip = m_EntityComponent;
            Entity = SpaceShip.Entity;
            LookingDestination = transform.position + new Vector3(0, 1, 0);
            if (m_NavMeshAgent)
            {
                m_NavMeshAgent.Warp(Head.position);
                //m_NavMeshAgent.updatePosition = false;
                //m_NavMeshAgent.updateRotation = false;
                //m_NavMeshAgent.updateUpAxis = false;
            }
        }

        private void Update()
        {
            if (SpaceShip.Entity.IsDead)
            {
                Animator.SetBool("Dead", true);
                return;
            }
            Stat acceleration = SpaceShip.Entity.GetStat("core.modtypes.ship.acceleration");
            Stat speed = SpaceShip.Entity.GetStat("core.modtypes.ship.movementspeed");
            if (m_NavMeshAgent)
            {
                m_NavMeshAgent.acceleration = acceleration.Calculated;
                m_NavMeshAgent.speed = speed.Calculated;
                if (m_NavMeshAgent.isActiveAndEnabled && !m_NavMeshAgent.isStopped)
                {
                    if (Target != null)
                    {
                        //AimTowardsTarget();
                    }
                    else
                    {
                        //if (m_NavMeshAgent.path.corners.Length > 1)
                            //AimTowardsPosition(m_NavMeshAgent.path.corners[1]);
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            Stat speed = SpaceShip.Entity.GetStat("core.modtypes.ship.movementspeed");
            speed.Current = NavMeshAgent.velocity.magnitude;
            Transform transform = Head;
            if (transform.position == LookingDestination)
            {
                LookingDestination = transform.position + transform.forward;
            }
        }
        public void Attack(EntityController origin, int index,params Muzzle[] muzzles)
        {
            m_EnemyActiveGemContainer.Spawn(origin, index,muzzles);
        }
        public void Attack(int index, params Muzzle[] muzzles)
        {
            m_EnemyActiveGemContainer.Spawn(this, index, muzzles);
        }
        public bool IsNearTarget(int index,float multiplier = 0.9f)
        {
            Vector3 distance = transform.position - Target.Head.position;
            float attackMax = GetAttackMaxDistance(index);
            attackMax *= attackMax;
            return distance.sqrMagnitude <= (attackMax * multiplier);
        }
        public float GetAttackMaxDistance(int index)
        {
            return m_EnemyActiveGemContainer.GetAttackMaxDistance(index);
        }
        public bool HasCooldown(EntityController origin, int index)
        {
            return m_EnemyActiveGemContainer.HasCooldown(origin,index);
        }
        public bool HasCooldown(int index)
        {
            return HasCooldown(this, index);
        }
        public bool IsAttackValid(int index)
        {
            return m_EnemyActiveGemContainer.IsIndexValid(index);
        }
        public bool RequiresTarget(int index)
        {
            return m_EnemyActiveGemContainer.RequiresTarget(index);
        }

        public float GetAttackPriority(int index)
        {
            return m_EnemyActiveGemContainer.GetAttackPriority(index,this,this.Target);
        }

        public bool AimTowardsTarget()
        {
            if (Target)
            {
                return AimTowardsPosition(Target.Head.position);
            }
            return false;
        }
        public bool AimTowardsPosition(Vector3 position)
        {
            Transform transform = Head;
            float distance = Vector3.Distance(transform.position, position);
            position = position - transform.position;
            if (position == Vector3.zero) return true;
            Quaternion targetRotation = Quaternion.LookRotation(position);
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
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(position), Time.deltaTime * 180.0f);
            LookingDestination = transform.forward * distance + transform.position;
        }

        public Vector3[] GetNavPoints()
        {
            if (!NavMeshAgent) return new Vector3[] { Head.position };
            return NavMeshAgent.path.corners;
        }
        public bool MoveTowardsTarget()
        {
            if (Target)
            {
                return MoveTowardsPosition(Target.Head.position);
            }
            return false;
        }
        public bool MoveTowardsPosition(Vector3 position)
        {
            if (!NavMeshAgent) return false;
            NavMeshAgent.destination = position;

            NavMeshHit hit;
            NavMesh.SamplePosition(position, out hit,(position - Head.position).magnitude,NavMesh.AllAreas);

            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(Head.position, hit.position, NavMesh.AllAreas, path))
            {
                NavMeshAgent.path = path;
                NavMeshAgent.isStopped = false;
                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }

        public void StopMove()
        {
            if (!NavMeshAgent) return;
            NavMeshAgent.isStopped = true;
        }
        public void ResumeMove()
        {
            if (!NavMeshAgent) return;
            NavMeshAgent.isStopped = false;
        }

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying || !m_NavMeshAgent) return;
            Vector3[] pathPoints = new Vector3[NavMeshAgent.path.corners.Length + 1];
            NavMeshAgent.path.corners.CopyTo(pathPoints, 0);
            pathPoints[pathPoints.Length - 1] = NavMeshAgent.pathEndPosition;
            Gizmos.color = Color.green;
            for (int i = 0; i < pathPoints.Length - 1; i++)
            {
                Gizmos.DrawLine(pathPoints[i], pathPoints[i + 1]);
            }
        }
        public Muzzle FindMuzzle(string name)
        {
            Muzzle muzzle;
            m_MuzzleDictionary.TryGetValue(name, out muzzle);
            return muzzle;
        }

        public Muzzle[] FindMuzzles(params string[] names)
        {
            List<Muzzle> muzzles = new List<Muzzle>();
            if (names != null)
            {
                foreach (string name in names)
                {
                    Muzzle muzzle = FindMuzzle(name);
                    if (!muzzle) continue;
                    muzzles.Add(muzzle);
                }
            }

            return muzzles.ToArray();
        }

        public Entity GetEntity(int index)
        {
            return m_EnemyActiveGemContainer.CalculateEntity(index);
        }
        #endregion
    }
}
