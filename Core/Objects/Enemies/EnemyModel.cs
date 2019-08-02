using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Logic.ActiveGemContainers;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Enemies
{
    public class EnemyModel : MonoBehaviour
    {
        [SerializeField] private EnemyActiveGemContainer m_EnemyActiveGemContainer = null;
        [SerializeField] private EntityBehaviour m_EntityBehaviour = null;
        [SerializeField] private EntityController m_EntityController = null;

        #region Properties
        public EnemyActiveGemContainer EnemyActiveGemContainer { get => m_EnemyActiveGemContainer; set => m_EnemyActiveGemContainer = value; }
        public EntityBehaviour EntityBehaviour { get => m_EntityBehaviour; set => m_EntityBehaviour = value; }
        public EntityController EntityController { get => m_EntityController; set => m_EntityController = value; }
        #endregion

        #region Methods
        #endregion
    }
}
