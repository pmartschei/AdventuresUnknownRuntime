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
    public abstract class BaseAction : ScriptableObject
    {
        [SerializeField] private int m_Priority = 0;

        #region Properties
        public abstract ActionType ActionType { get; }
        public int Priority { get => m_Priority; set => m_Priority = value; }
        public ModType ModType { get; set; }
        public BaseAction Root { get; set; }
        #endregion

        #region Methods
        public virtual void Initialize(ModType modType)
        {
            this.ModType = modType;
            this.Root = this;
        }
        public virtual void Initialize(Entity activeStats)
        {
        }
        public abstract void Notify(Entity activeStats, ActionContext actionContext);
        #endregion
    }
}
