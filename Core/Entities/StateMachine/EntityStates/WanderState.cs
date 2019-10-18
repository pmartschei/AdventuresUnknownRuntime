using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces;
using AdventuresUnknownSDK.Core.Entities.StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

namespace AdventuresUnknownSDK.Core.Entities.StateMachine.EntityStates
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/EntityStates/WanderState", fileName = "WanderState.asset")]
    public class WanderState : EntityState
    {
        [SerializeField] float m_MaxMoveRadius = 4.0f;
        [SerializeField] float m_MinWanderTimer =1.0f;
        [SerializeField] float m_MaxWanderTimer = 3.0f;
        [SerializeField] int m_RecalcPathCount = 0;
        [SerializeField] bool m_NewDestinationOnPathFinish = true;

        private float m_WanderTimer = 0.0f;

        #region Properties
        public float MaxMoveRadius { get => m_MaxMoveRadius; set => m_MaxMoveRadius = value; }
        public float MinWanderTimer { get => m_MinWanderTimer; set => m_MinWanderTimer = value; }
        public float MaxWanderTimer { get => m_MaxWanderTimer; set => m_MaxWanderTimer = value; }
        public int RecalcPathCount { get => m_RecalcPathCount; set => m_RecalcPathCount = value; }
        public bool NewDestinationOnPathFinish { get => m_NewDestinationOnPathFinish; set => m_NewDestinationOnPathFinish = value; }
        #endregion

        #region Methods
        public override void OnEnter()
        {
            base.OnEnter();
        }
        public override void Update()
        {
            base.Update();
            if (CommonComponents.TranslationalController == null) return;

            m_WanderTimer -= Time.deltaTime;
            if (m_WanderTimer <= 0.0f || (m_NewDestinationOnPathFinish && CommonComponents.EnemyController.NavMeshAgent.remainingDistance <= CommonComponents.EnemyController.NavMeshAgent.stoppingDistance))
            {
                m_WanderTimer = UnityEngine.Random.Range(m_MinWanderTimer, m_MaxWanderTimer);
                int recalcCount = Mathf.Max(m_RecalcPathCount + 1, 1);
                for(int i = 0; i < recalcCount; i++)
                {
                    if (Wander()) break;
                }
            }
        }
        protected virtual bool Wander()
        {
            Vector3 randomPosition = UnityEngine.Random.insideUnitSphere * m_MaxMoveRadius;

            randomPosition += gameObject.transform.position;

            return CommonComponents.TranslationalController.MoveTowardsPosition(randomPosition);
        }

        //Vector3 GetRandomLocation()
        //{
        //    NavMeshTriangulation navMeshData = NavMesh.CalculateTriangulation();
        //    int maxIndices = navMeshData.indices.Length - 3;
        //    // Pick the first indice of a random triangle in the nav mesh
        //    int firstVertexSelected = UnityEngine.Random.Range(0, maxIndices);
        //    int secondVertexSelected = UnityEngine.Random.Range(0, maxIndices);
        //    //Spawn on Verticies
        //    Vector3 point = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];

        //    Vector3 firstVertexPosition = navMeshData.vertices[navMeshData.indices[firstVertexSelected]];
        //    Vector3 secondVertexPosition = navMeshData.vertices[navMeshData.indices[secondVertexSelected]];
        //    //Eliminate points that share a similar X or Z position to stop spawining in square grid line formations
        //    if ((int)firstVertexPosition.x == (int)secondVertexPosition.x ||
        //        (int)firstVertexPosition.z == (int)secondVertexPosition.z
        //        )
        //    {
        //        point = GetRandomLocation(); //Re-Roll a position - I'm not happy with this recursion it could be better
        //    }
        //    else
        //    {
        //        // Select a random point on it
        //        point = Vector3.Lerp(
        //                                        firstVertexPosition,
        //                                        secondVertexPosition, //[t + 1]],
        //                                        UnityEngine.Random.Range(0.05f, 0.95f) // Not using Random.value as clumps form around Verticies 
        //                                    );
        //    }
        //    //Vector3.Lerp(point, navMeshData.vertices[navMeshData.indices[t + 2]], Random.value); //Made Obsolete

        //    return point;
        //}
        #endregion
    }
}
