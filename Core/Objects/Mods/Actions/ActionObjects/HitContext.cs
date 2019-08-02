using AdventuresUnknownSDK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects
{
    public class HitContext : ActionContext
    {

        private EntityBehaviour m_Source;
        private EntityBehaviour m_Target;
        private Entity m_OffensiveEntity;
        private Entity m_DefensiveEntity;
        private bool m_IsProtected = false;
        private ProtectionCause m_ProtectionCause = null;

        #region Properties
        public EntityBehaviour Target { get => m_Target; }
        public EntityBehaviour Source { get => m_Source; }
        public Entity OffensiveEntity { get => m_OffensiveEntity; }
        public Entity DefensiveEntity { get => m_DefensiveEntity; }
        public bool IsProtected { get => m_IsProtected; set => m_IsProtected = value; }
        public ProtectionCause ProtectionCause { get => m_ProtectionCause; set => m_ProtectionCause = value; }
        #endregion

        #region Methods
        public HitContext(EntityBehaviour source, EntityBehaviour target)
        {
            this.m_Source = source;
            this.m_Target = target;
            this.m_OffensiveEntity = new Entity();
            this.m_DefensiveEntity = new Entity();
        }
        public void NotifyTarget(ActionType actionType)
        {
            m_Target.Entity.Notify(actionType, this);
        }
        public void NotifyOffensiveEntity(ActionType actionType)
        {
            m_OffensiveEntity.Notify(actionType, this);
        }
        public void NotifyDefensiveEntity(ActionType actionType)
        {
            m_DefensiveEntity.Notify(actionType, this);
        }
        public void NotifySource(ActionType actionType)
        {
            m_Source.Entity.Notify(actionType, this);
        }

        #endregion
    }
}
