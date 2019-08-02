using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
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

        #region Properties

        #endregion

        #region Methods

        public override IEnumerator Activate(EntityController controller, Entity stats, Vector3 origin, Vector3 destination)
        {
            SpawnSingleInstance(controller, stats, origin, destination);
            yield return null;
        }

        protected override void AttackUpdate()
        {

        }
        #endregion
    }
}
