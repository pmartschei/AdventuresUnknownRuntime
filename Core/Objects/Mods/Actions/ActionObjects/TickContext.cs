using AdventuresUnknownSDK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects
{
    public class TickContext : ActionContext
    {

        private float m_Time;

        #region Properties
        public float Time { get => m_Time; }
        #endregion

        #region Methods
        public TickContext(float time)
        {
            this.m_Time = time;
        }

        #endregion
    }
}
