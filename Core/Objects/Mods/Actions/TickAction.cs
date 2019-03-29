using AdventuresUnknownSDK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions
{
    public abstract class TickAction : ScriptableObject
    {


        #region Properties

        #endregion

        #region Methods
        public abstract void OnTick(IActiveStat iActiveStat, float time, ModType modType);
        #endregion
    }
}
