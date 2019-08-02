using AdventuresUnknownSDK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects
{
    public class CooldownContext : ActionContext
    {
        private bool m_CanUse = true;
        private Entity m_Entity;


        #region Properties
        public bool CanUse { get => m_CanUse; set => m_CanUse = value; }
        public Entity Entity { get => m_Entity; set => m_Entity = value; }
        #endregion

        #region Methods
        public CooldownContext() : this(new Entity())
        {
        }
        public CooldownContext(Entity entity)
        {
            m_Entity = entity;
        }
        #endregion
    }
}
