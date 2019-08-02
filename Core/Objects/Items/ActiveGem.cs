using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Items
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/ActiveGem", fileName = "ActiveGem.asset")]
    public class ActiveGem : Gem
    {

        [SerializeField] private GenericAttack m_GenericAttack = null;

        #region Properties

        #endregion

        #region Methods
        public virtual void Activate(EntityController entityController,Entity stats,Vector3 origin,Vector3 destination)
        {
            if (m_GenericAttack == null)
            {
                GameConsole.LogWarningFormat("ActiveGem {0} has no attack prefab", this.Identifier);
                return;
            }
            entityController.StartCoroutine(m_GenericAttack.Activate(entityController, stats, origin, destination));
        }

        public virtual float GetMaxSkillDistance(Entity stats)
        {
            Collider[] colliders = m_GenericAttack.GetComponentsInChildren<Collider>();

            float maxDistance = 0.0f;

            foreach(Collider collider in colliders)
            {
                maxDistance = Mathf.Max(collider.bounds.center.x + collider.bounds.extents.x);
            }

            Stat projectileSpeed = stats.GetStat("core.modtypes.skills.projectilespeed");
            Stat duration = stats.GetStat("core.modtypes.skills.duration");

            float travelDistance = duration.Calculated * projectileSpeed.Calculated;

            return maxDistance + travelDistance;
        }
        #endregion
    }
}
