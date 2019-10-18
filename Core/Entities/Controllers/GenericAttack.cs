using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Objects.Pool;
using AdventuresUnknownSDK.Core.Utils.Extensions;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PoolDescription))]
    public abstract class GenericAttack : EntityController
    {
        private Vector3 m_Origin = Vector3.zero;
        private Vector3 m_Destination = Vector3.zero;
        private Vector3 m_Direction = Vector3.zero;
        private float m_Angle = 0.0f;
        private ActiveGem[] m_SecondaryActiveGems = null;
        private float m_Level = 0.0f;

        private Entity m_FrozenStats = null;

        [SerializeField] private GameObject m_MuzzlePrefab = null;
        [SerializeField] private ModTypeIdentifier[] m_ResetModifiersOnActivate = new ModTypeIdentifier[0];
        [SerializeField] private Objects.Mods.Attribute[] m_AddAttributesOnActivate = new Objects.Mods.Attribute[0];
        [SerializeField] private bool m_AIRequiresTarget = false;

        private int m_Coroutines = 0;

        private List<TimerObject> m_HitTimer = new List<TimerObject>();

        #region Properties
        public ActivationParameters ActivationParameters { get; private set; }
        public Vector3 Destination { get => m_Destination;
            set
            {
                if (m_Destination != value)
                {
                    m_Destination = value;
                    LookingDestination = m_Destination;
                    RecalculateDirectionAndAngle();
                }
            }
        }
        public Vector3 Origin { get => m_Origin; set
            {
                if (m_Origin != value)
                {
                    m_Origin = value;
                    RecalculateDirectionAndAngle();
                }
            }
        }
        public float Angle { get => m_Angle; }
        public Vector3 Direction { get => m_Direction; }
        public bool IsDestroyed { get; set; }
        public Rigidbody Rigidbody { get; private set; }
        protected PoolDescription PoolDescription { get; private set; }
        public bool AIRequiresTarget { get => m_AIRequiresTarget; set => m_AIRequiresTarget = value; }
        public GameObject MuzzlePrefab { get => m_MuzzlePrefab; set => m_MuzzlePrefab = value; }
        protected float StartVelocity { get; set; }
        #endregion
        #region Methods
        public virtual IEnumerator Activate(ActivationParameters activationParameters)
        {
            yield return null;
        }

        public virtual void PreActivate(ActivationParameters activationParameters)
        {
            Entity stats = activationParameters.Stats;
            foreach (ModTypeIdentifier modTypeIdentifier in m_ResetModifiersOnActivate)
            {
                if (!modTypeIdentifier.ConsistencyCheck()) continue;
                stats.GetStat(modTypeIdentifier.Identifier).RemoveAllStatModifiers();
            }
            foreach (Objects.Mods.Attribute attribute in m_AddAttributesOnActivate)
            {
                if (!attribute.ConsistencyCheck()) continue;
                stats.GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.Value(activationParameters.Level), attribute.ModBase.CalculationType, this));
            }
            foreach (Objects.Mods.Attribute attribute in activationParameters.ActiveGem.Attributes)
            {
                stats.GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.Value(activationParameters.Level), attribute.ModBase.CalculationType, this));
            }
            AttackContext attackContext = new AttackContext(activationParameters.ActiveGem);
            stats.Notify(ActionTypeManager.AttackApply, attackContext);
            stats.Notify(ActionTypeManager.AttackGeneration, attackContext);
        }
        protected GenericAttack SpawnSingleInstance(ActivationParameters activationParameters, Vector3 origin, Vector3 destination, Muzzle muzzle = null)
        {
            ActivationParameters = activationParameters;
            float startVelocity = 0.0f;
            if (muzzle != null)
            {
                float originY = origin.y;
                float destinationY = destination.y;
                Vector3 diff = (muzzle.MuzzleTransform.position - origin);
                origin += diff;
                destination += diff;
                destination.y = destinationY;
                origin.y = originY;
                if (muzzle.CanRotate)
                {
                    muzzle.transform.rotation = Quaternion.LookRotation(destination - muzzle.transform.position);
                }
            }
            GameObject go = PoolManager.Instantiate(this.gameObject,origin,this.transform.rotation,UIManager.AttacksTransform);
            //GameObject go = Instantiate(this.gameObject,origin,this.transform.rotation,UIManager.AttacksTransform);
            GenericAttack instance = go.GetComponent<GenericAttack>();
            EntityDescription entityDescription = instance.Entity.Description;
            EntityController controller = activationParameters.EntityController;

            entityDescription.EntityType = EntityType.Attack;
            int layer = controller.gameObject.layer;
            if (controller.SpaceShip)
            {
                layer = controller.SpaceShip.gameObject.layer;
                entityDescription.IsPlayer = controller.SpaceShip.Entity.Description.IsPlayer;
                if (controller.Entity.Description.EntityType == EntityType.SpaceShip)
                {
                    startVelocity = activationParameters.EntityController.Entity.GetStat("core.modtypes.ship.movementspeed").Current;
                    if (muzzle && m_MuzzlePrefab)
                    {
                        //GameObject muzzleEffect = Instantiate(m_MuzzlePrefab, muzzle.MuzzleTransform.position,
                        //   muzzle.MuzzleTransform.rotation, muzzle.MuzzleTransform);
                        //muzzleEffect.transform.localScale = muzzle.MuzzleTransform.localScale;
                    }
                }
            }
            else
            {
                GenericAttack genericAttack = controller as GenericAttack;
                if (genericAttack)
                {
                    entityDescription.IsPlayer = genericAttack.Entity.Description.IsPlayer;
                }
            }

            instance.m_SecondaryActiveGems = activationParameters.SecondaryActiveGems;
            instance.IsDestroyed = false;
            instance.m_HitTimer.Clear();
            instance.StartVelocity = startVelocity;
            instance.m_Level = activationParameters.Level;
            instance.gameObject.transform.MoveToLayer(layer);
            instance.SpaceShip = controller.SpaceShip;
            instance.Origin = origin;
            instance.Destination = destination;
            instance.m_FrozenStats = new Entity();
            instance.m_FrozenStats.CopyFrom(activationParameters.FrozenStats);
            instance.Entity.Reset();
            instance.Entity.CopyFrom(activationParameters.Stats);
            instance.gameObject.SetActive(true);
            instance.Rigidbody.velocity = Vector3.zero;
            return instance;
        }

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            PoolDescription = GetComponent<PoolDescription>();
        }

        private void Update()
        {
            if (IsDestroyed) return;
            for (int i = 0; i < m_HitTimer.Count; i++)
            {
                m_HitTimer[i].Update(Time.fixedDeltaTime);
                if (m_HitTimer[i].IsFinished())
                {
                    m_HitTimer.RemoveAt(i);
                    i--;
                }
            }
            if (!CheckDuration()) return;
            AttackUpdate();
        }

        private void FixedUpdate()
        {
            if (IsDestroyed) return;
            AttackFixedUpdate();
        }
        protected virtual void AttackUpdate()
        {

        }
        protected virtual void AttackFixedUpdate()
        {

        }
        private void RecalculateDirectionAndAngle()
        {
            this.m_Direction = (Destination - Origin).normalized;
            this.m_Angle = Mathf.Atan2(this.Direction.z, this.Direction.x);
            transform.rotation = Quaternion.LookRotation(Direction);
            //this.transform.rotation = Quaternion.AngleAxis(m_Angle * Mathf.Rad2Deg - 90.0f, Vector3.up);
        }

        protected void ExecuteCoroutine(IEnumerator ie)
        {
            m_Coroutines++;
            StartCoroutine(ie);
            m_Coroutines--;
        }

        protected virtual void DestroyAttack()
        {
            IsDestroyed = true;
            StartCoroutine(DestroyCoroutine());
        }
        protected virtual bool CheckDuration()
        {
            Stat duration = Entity.GetStat("core.modtypes.skills.duration");
            duration.Current -= Time.fixedDeltaTime;
            if (duration.Current <= 0.0f)
            {
                DestroyAttack();
                return false;
            }
            return true;
        }

        private IEnumerator DestroyCoroutine()
        {
            m_Coroutines++;
            yield return OnAttackDestroy();
            m_Coroutines--;
            yield return new WaitWhile(HasCoroutines);
            if (PoolDescription)
            {
                PoolDescription.DisableGameObject(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            yield return null;
        }

        protected virtual void ActivateSecondaryActiveGem(int index)
        {
            if (m_SecondaryActiveGems == null) return;
            if (m_SecondaryActiveGems.Length <= index) return;
            ActiveGem activeGem = m_SecondaryActiveGems[index];
            if (activeGem == null) return;
            Entity entity = new Entity();
            entity.CopyFrom(m_FrozenStats);
            entity.Notify(ActionTypeManager.AttackGeneration, new AttackContext(activeGem));
            activeGem.Activate(this, entity, m_Level);
        }

        private bool HasCoroutines()
        {
            return m_Coroutines > 0;
        }

        protected virtual IEnumerator OnAttackDestroy()
        {
            yield return null;
        }
        protected virtual void OnAttackHit(HitContext hitContext)
        {
            if (hitContext.IsProtected)
            {
                if (hitContext.ProtectionCause.Value == ProtectionCauseManager.Block.Value)
                {
                    OnBlock(hitContext);
                }
                else
                {
                    OnProtection(hitContext.ProtectionCause, hitContext);
                }
            }
            else
            {
                m_HitTimer.Add(new TimerObject(hitContext.Target.Entity, hitContext.OffensiveEntity.GetStat("core.modtypes.skills.hittimer").Calculated));
                hitContext.NotifyOffensiveEntity(ActionTypeManager.HitApply);
            }
        }
        protected virtual void OnBlock(HitContext hitContext)
        {

        }
        protected virtual void OnProtection(ProtectionCause protectionCause, HitContext hitContext)
        {

        }

        private void OnTriggerStay(Collider other)
        {
            OnTriggerEnter(other);   
        }
        private void OnTriggerEnter(Collider other)
        {
            CollideWith(other);
        }
        protected virtual void CollideWith(Collider collider)
        {
            if (IsDestroyed) return;
            EntityController controller = collider.GetComponentInParent<EntityController>();
            if (!controller || !controller.SpaceShip || controller.Entity.Description.EntityType != EntityType.SpaceShip) return;
            HandleShipCollision(controller.SpaceShip);
        }
        protected virtual void HandleShipCollision(EntityBehaviour enemySpaceShip)
        {
            if (!enemySpaceShip) return;
            if (enemySpaceShip.gameObject.layer == this.gameObject.layer) return;
            if (enemySpaceShip.Entity.IsDead) return;
            if (enemySpaceShip.Entity.GetStat("core.modtypes.ship.life").Current <= 0.0f) return;
            if (m_HitTimer.Contains(new TimerObject(enemySpaceShip.Entity,0.0f),new TimerObject.CompareSource())) return;
            HitContext hitContext = new HitContext(this.SpaceShip, enemySpaceShip);
            Entity.Notify(ActionTypeManager.HitGenerationOffensive, hitContext);
            hitContext.NotifyTarget(ActionTypeManager.HitGenerationDefensive);
            hitContext.NotifyOffensiveEntity(ActionTypeManager.HitCalculation);
            hitContext.NotifyDefensiveEntity(ActionTypeManager.HitCalculation);
            OnAttackHit(hitContext);
        }

        public virtual float GetAIPriority(EntityController controller, Entity stats, EntityController target)
        {
            return 1.0f;
        }
        public virtual float GetMaxSkillDistance(Entity stats)
        {
            Collider[] colliders = GetComponentsInChildren<Collider>();

            float maxDistance = 0.0f;

            foreach (Collider collider in colliders)
            {
                maxDistance = Mathf.Max(maxDistance, collider.bounds.center.x + collider.bounds.extents.x);
            }

            return maxDistance;
        }

        #endregion
    }
}
