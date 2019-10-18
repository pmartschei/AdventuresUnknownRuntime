using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu]
    public class OrbitParentState : EntityState
    {
        [SerializeField] private float m_MaxOrbitRadius = 5.0f;
        [SerializeField] private float m_MinOrbitRadius = 3.0f;
        [SerializeField] private float m_OrbitSpeed = 40.0f;
        [SerializeField] private float m_OrbitSpeedMultiplier = 1.0f;
        [SerializeField] private EntityState m_OnTargetFoundState = null;

        private float m_Angle = 0.0f;
        private int m_Seed = 0;
        private float m_OrbitPerSecond = 360.0f;
        #region Properties
        #endregion

        #region Methods
        public override void OnEnter()
        {
            base.OnEnter();
            m_Seed = Random.Range(0, int.MaxValue);
        }
        public override void Update()
        {
            base.Update();

            if (CommonComponents.TranslationalController == null) return;
            if (CommonComponents.EntityController.Target && m_OnTargetFoundState)
            {
                EntityStateMachine.SetNextState(m_OnTargetFoundState);
                return; 
            }

            float radius = Mathf.PerlinNoise(Age, m_Seed) * (m_MaxOrbitRadius - m_MinOrbitRadius);
            radius = Mathf.Clamp(radius + m_MinOrbitRadius, m_MinOrbitRadius, m_MaxOrbitRadius);
            if (radius != 0.0f)
            {
                m_OrbitPerSecond = m_OrbitSpeed * CommonComponents.EnemyController.Entity.GetStat("core.modtypes.ship.movementspeed").Current / radius;
                m_Angle += Time.deltaTime * m_OrbitPerSecond * m_OrbitSpeedMultiplier;
            }

            Vector3 targetPosition = Vector3.forward * radius;
            targetPosition = Quaternion.Euler(0, m_Angle, 0) * targetPosition;
            targetPosition += CommonComponents.EnemyController.Entity.Description.Parent.EntityController.Head.position;
            CommonComponents.TranslationalController.MoveTowardsPosition(targetPosition);
        }
        #endregion
    }
}
