using AdventuresUnknownSDK.Core.Objects.Enemies;
using System;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities
{
    [Serializable]
    public class EntityDescription
    {
        [SerializeField] private bool m_IsPlayer = false;
        [SerializeField] private Enemy m_Enemy = null;
        [SerializeField] private EntityType m_EntityType = EntityType.SpaceShip;
        [SerializeField] private bool m_IsMinion = false;
        [SerializeField] private GameObject m_Parent = null;
        #region Properties
        public bool IsPlayer { get => m_IsPlayer; set => m_IsPlayer = value; }
        public Enemy Enemy { get => m_Enemy; set => m_Enemy = value; }
        public EntityType EntityType { get => m_EntityType; set => m_EntityType = value; }
        public bool IsMinion { get => m_IsMinion; set => m_IsMinion = value; }
        public GameObject Parent { get => m_Parent; set => m_Parent = value; }
        #endregion
    }
}