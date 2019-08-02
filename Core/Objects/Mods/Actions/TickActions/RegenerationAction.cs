using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.TickActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Tick/RegenerationAction", fileName = "RegenerationAction.asset")]
    public class RegenerationAction : TickAction
    {
        [SerializeField] private ModTypeIdentifier m_Destination = null;

        #region Methods
        public override void Notify(Entity activeStats, ActionContext context)
        {
            TickContext tickContext = context as TickContext;
            if (tickContext == null) return;
            Stat destination = activeStats.GetStat(m_Destination.Identifier);
            Stat regeneration = activeStats.GetStat(ModType.Identifier);
            if (destination == null || regeneration == null) return;
            destination.Current += regeneration.Calculated * tickContext.Time;
        }
        #endregion
    }
}
