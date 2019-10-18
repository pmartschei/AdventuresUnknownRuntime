using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EntityStates/SearchTargetState", fileName = "SearchTargetState.asset")]
    public class SearchTargetState : EntityState
    {

        [SerializeField] EntityState m_NextState = null;
        [SerializeField] float m_SearchRadius = 25.0f;

        #region Properties
        #endregion

        #region Methods
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public override void Update()
        {
            base.Update();
            Collider[] colliders = Physics.OverlapSphere(this.gameObject.transform.position, m_SearchRadius, ~(1 << this.gameObject.layer));
            List<EntityController> availableTargets = new List<EntityController>();

            foreach (Collider collider in colliders)
            {
                if (Physics.GetIgnoreLayerCollision(collider.gameObject.layer, this.gameObject.layer)) continue;
                EntityController entityController = collider.gameObject.GetComponentInParent<EntityController>();
                if (!entityController) continue;
                if (entityController.Entity.Description.EntityType != EntityType.SpaceShip) continue;
                availableTargets.Add(entityController);
            }
            CommonComponents.EntityController.Target = FindBestTarget(availableTargets);
            if (CommonComponents.EntityController.Target && m_NextState != null)
            {
                EntityStateMachine.SetNextState(m_NextState);
                return;
            }
        }

        public virtual EntityController FindBestTarget(List<EntityController> targets)
        {
            if (targets.Count == 0) return null;
            return targets[0];
        }

        public override void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(this.gameObject.transform.position, m_SearchRadius);
        }
        #endregion
    }
}
