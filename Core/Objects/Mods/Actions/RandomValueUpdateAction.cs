using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/TickActions/RandomValueUpdateAction", fileName = "RandomValueUpdateAction.asset")]
    public class RandomValueUpdateAction : TickAction
    {


        #region Properties

        #endregion

        #region Methods
        public override void OnTick(IActiveStat iActiveStat, float time, ModType modType)
        {
            iActiveStat.GetStat(modType.Identifier).Current += UnityEngine.Random.Range(-1.0f, 1.0f) * time;
        }
        #endregion
    }
}
