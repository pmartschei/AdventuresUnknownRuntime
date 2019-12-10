using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects
{
    public class AuraContext : ActionContext
    {
        private object m_Source;
        #region Properties
        public object Source { get => m_Source; }
        #endregion
        #region Methods
        public AuraContext(object source)
        {
            m_Source = source;
        }
        #endregion
    }
}
