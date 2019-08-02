using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Enemies.Behaviours
{
    public class MoveToTargetBehaviour : EnemyStateMachineBehaviour
    {
        [SerializeField] private ModTypeIdentifier m_SpeedMod = null;

        #region Properties

        #endregion

        #region Methods
        public override void OnUpdate()
        {
            if (!EnemyController) return;
            if (!EnemyController.Target) return;
            MoveTo(EnemyController.Target.transform.position);
        }
        private void MoveTo(Vector3 position)
        {
            Vector3 distance = position - EnemyController.transform.position;
            Stat speed = EnemyController.SpaceShip.Entity.GetStat(m_SpeedMod.Identifier);
            EnemyController.RigidBody.AddForce(distance * speed.Calculated);
        }
        #endregion
    }
}
