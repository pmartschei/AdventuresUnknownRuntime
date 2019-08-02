using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects
{
    public class AttackContext : ActionContext
    {
        private ActiveGem m_ActiveGem;
        #region Properties
        public ActiveGem ActiveGem { get => m_ActiveGem; }
        #endregion
        #region Methods
        public AttackContext(ActiveGem activeGem)
        {
            m_ActiveGem = activeGem;
        }
        #endregion
    }
}
