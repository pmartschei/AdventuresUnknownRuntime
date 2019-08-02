using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Enemies.Behaviours
{
    public class AimAtTargetBehaviour : EnemyStateMachineBehaviour
    {

        [SerializeField] private float m_RotationalSpeed = 360.0f;
        #region Properties

        #endregion

        #region Methods
        public override void OnUpdate()
        {
            if (!EnemyController) return;
            if (!EnemyController.Target) return;
            LookAt(EnemyController.Target.transform.position);
        }
        private void LookAt(Vector3 position)
        {
            float z = Mathf.Atan2((position.y - EnemyController.transform.position.y),
                (position.x - EnemyController.transform.position.x)) * Mathf.Rad2Deg;
            EnemyController.transform.rotation = Quaternion.RotateTowards(
                EnemyController.transform.rotation,
                Quaternion.Euler(EnemyController.transform.rotation.eulerAngles.x, EnemyController.transform.rotation.eulerAngles.y, z - 90.0f),
                Time.deltaTime * m_RotationalSpeed);
        }
        #endregion
    }
}
