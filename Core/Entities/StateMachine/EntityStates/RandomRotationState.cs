using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces;
using AdventuresUnknownSDK.Core.Entities.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = UnityEngine.Random;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EntityStates/RandomRotationState", fileName = "RandomRotationState.asset")]
    public class RandomRotationState : EntityState
    {
        [SerializeField] private float m_TimerMin = 3.0f;
        [SerializeField] private float m_TimerMax = 5.0f;
        private float m_CurrentTimer = 0.0f;
        private Vector3 m_LookingDirection = Vector3.zero;
        #region Properties

        #endregion

        #region Methods

        public override void OnEnter()
        {
            base.OnEnter();
            m_CurrentTimer = Random.Range(m_TimerMin, m_TimerMax);
        }
        public override void Update()
        {
            base.Update();
            if (CommonComponents.RotationalController == null) return;
            if (CommonComponents.EntityController.Target != null) return;
            m_CurrentTimer -= Time.deltaTime;
            if (m_CurrentTimer <= 0.0f)
            {
                m_CurrentTimer = Random.Range(m_TimerMin, m_TimerMax);
                Vector2 circle = UnityEngine.Random.insideUnitCircle;
                m_LookingDirection = new Vector3(circle.x, 0.0f, circle.y);
            }
            CommonComponents.RotationalController.AimTowardsPosition(this.gameObject.transform.position + m_LookingDirection);
        }
        #endregion
    }
}
