using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Logic.Attacks
{
    public class ExplosionAttack : GenericAttack
    {
        [SerializeField] private float m_ExplosionRange = 1.0f;

        private bool m_DidExplode = false;
        #region Properties

        #endregion

        #region Methods

        public override IEnumerator Activate(ActivationParameters activationParameters)
        {
            PreActivate(activationParameters);
            SpawnSingleInstance(activationParameters, activationParameters.EntityController.transform.position, activationParameters.EntityController.LookingDestination);
            yield return null;
        }
        public override void PreActivate(ActivationParameters activationParameters)
        {
            base.PreActivate(activationParameters);
            //Make sure the explosion is longer than the particle system (10 seconds should be enough)
            activationParameters.Stats.GetStat("core.modtypes.skills.duration").AddStatModifier(new StatModifier(10.0f, Objects.Mods.CalculationType.Flat, this));
        }
        protected override void AttackFixedUpdate()
        {
            base.AttackFixedUpdate();
            if (!m_DidExplode)
            {
                m_DidExplode = true;

                Collider[] colliders;
                colliders = Physics.OverlapSphere(Origin, m_ExplosionRange, ~(1 << this.gameObject.layer));
                foreach(Collider collider in colliders)
                {
                    if (Physics.GetIgnoreLayerCollision(collider.gameObject.layer, this.gameObject.layer)) continue;
                    CollideWith(collider);
                }
            }
        }
        public override float GetMaxSkillDistance(Entity stats)
        {
            return m_ExplosionRange;
        }
        #endregion
    }
}
