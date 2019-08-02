using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
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
    public abstract class GenericAttack : EntityController
    {
        [SerializeField] private Entity m_Entity = null;
        private Vector3 m_Origin = Vector3.zero;
        private Vector3 m_Destination = Vector3.zero;
        private Vector3 m_Direction = Vector3.zero;
        private float m_Angle = 0.0f;

        [SerializeField] private ModTypeIdentifier[] m_ResetModifiersOnActivate = new ModTypeIdentifier[0];
        [SerializeField] private Objects.Mods.Attribute[] m_AddAttributesOnActivate = new Objects.Mods.Attribute[0];

        private int m_Coroutines = 0;

        #region Properties
        public Vector3 Destination { get => m_Destination;
            set
            {
                if (m_Destination != value)
                {
                    m_Destination = value;
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
        public Entity Entity { get => m_Entity; }
        public bool IsDestroyed { get; set; }
        public Rigidbody Rigidbody { get; private set; }
        #endregion
        #region Methods
        public virtual IEnumerator Activate(EntityController controller, Entity stats, Vector3 origin, Vector3 destination)
        {
            foreach(ModTypeIdentifier modTypeIdentifier in m_ResetModifiersOnActivate)
            {
                if (!modTypeIdentifier.ConsistencyCheck()) continue;
                stats.GetStat(modTypeIdentifier.Identifier).RemoveAllStatModifiers();
            }
            foreach(Objects.Mods.Attribute attribute in m_AddAttributesOnActivate)
            {
                if (!attribute.ConsistencyCheck()) continue;
                stats.GetStat(attribute.ModBase.ModTypeIdentifier).AddStatModifier(new StatModifier(attribute.Value(0.0f), attribute.ModBase.CalculationType, this));
            }
            yield return null;
        }
        protected abstract void AttackUpdate();

        protected GenericAttack SpawnSingleInstance(EntityController controller,Entity stats, Vector3 origin,Vector3 destination)
        {
            GenericAttack instance = Instantiate(this,origin,this.transform.rotation);
            int layer = controller.gameObject.layer;
            if (controller.SpaceShip)
            {
                layer = controller.SpaceShip.gameObject.layer;
            }
            instance.gameObject.SetActive(true);
            instance.gameObject.transform.MoveToLayer(layer);
            instance.SpaceShip = controller.SpaceShip;
            instance.Origin = origin;
            instance.Destination = destination;
            //instance.m_Entity = stats.Copy();
            instance.m_Entity.CopyFrom(stats);
            return instance;
        }
        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if (IsDestroyed) return;
            if (!CheckDuration()) return;
            AttackUpdate();
        }

        private void RecalculateDirectionAndAngle()
        {
            this.m_Direction = (Destination - Origin).normalized;
            this.m_Angle = Mathf.Atan2(this.Direction.y, this.Direction.x);
            this.transform.rotation = Quaternion.AngleAxis(m_Angle * Mathf.Rad2Deg, Vector3.forward);
        }

        protected void ExecuteCoroutine(IEnumerator ie)
        {
            m_Coroutines++;
            SpaceShip.StartCoroutine(ie);
            m_Coroutines--;
        }

        protected virtual void DestroyAttack()
        {
            IsDestroyed = true;
            this.gameObject.SetActive(false);
            SpaceShip.StartCoroutine(DestroyCoroutine());
        }
        protected virtual bool CheckDuration()
        {
            Stat duration = Entity.GetStat("core.modtypes.skills.duration");
            duration.Current -= Time.deltaTime;
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
            Destroy(this.gameObject);
            yield return null;
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
            if (!controller || !controller.SpaceShip) return;
            HandleShipCollision(controller.SpaceShip);
        }
        protected virtual void HandleShipCollision(EntityBehaviour spaceShip)
        {
            if (!spaceShip) return;
            if (!this.SpaceShip) return;
            if (spaceShip.gameObject.layer == this.gameObject.layer) return;
            if (spaceShip.Entity.GetStat("core.modtypes.ship.life").Current <= 0.0f) return;
            HitContext hitContext = new HitContext(this.SpaceShip, spaceShip);
            m_Entity.Notify(ActionTypeManager.HitGeneration, hitContext);
            hitContext.NotifyTarget(ActionTypeManager.HitGeneration);
            hitContext.NotifyOffensiveEntity(ActionTypeManager.HitCalculation);
            hitContext.NotifyDefensiveEntity(ActionTypeManager.HitCalculation);
            OnAttackHit(hitContext);
        }
        
        #endregion
    }
}
