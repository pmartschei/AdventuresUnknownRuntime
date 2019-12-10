using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions
{
    public abstract class ConditionAction : ScriptableObject
    {
        #region Properties
        public ModType ModType { get; set; }
        #endregion

        #region Methods
        public virtual void Initialize(ModType modType)
        {
            this.ModType = modType;
        }
        public abstract bool Notify(Entity activeStats, ActionContext actionContext);
        #endregion
    }
}
