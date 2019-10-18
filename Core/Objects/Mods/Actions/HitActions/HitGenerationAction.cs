using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.HitActions
{
    public abstract class HitGenerationAction : BaseAction
    {

        [SerializeField] private HitType m_Type = HitType.OffensiveEntity;
        #region Properties
        public override ActionType ActionType { get => m_Type == HitType.OffensiveEntity ? ActionTypeManager.HitGenerationOffensive : ActionTypeManager.HitGenerationDefensive; }
        public HitType Type { get => m_Type; set => m_Type = value; }
        #endregion

        #region Methods
        protected Entity GetTypeEntity(HitContext hitContext)
        {
            Entity entity = hitContext.DefensiveEntity;
            if (Type == HitType.OffensiveEntity)
            {
                entity = hitContext.OffensiveEntity;
            }
            return entity;
        }
        #endregion
    }
}
