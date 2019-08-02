using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions
{
    public abstract class MultipleBaseAction : BaseAction
    {
        [SerializeField] private ActionType m_ActionType = null;

        #region Properties
        public override ActionType ActionType { get => m_ActionType; }
        #endregion

        #region Methods

        #endregion
    }
}
