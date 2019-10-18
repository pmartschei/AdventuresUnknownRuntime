using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.AttackActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/AttackGeneration/BloodMagicAction", fileName = "BloodMagicAction.asset")]
    public class BloodMagicAction : AttackApplyAction
    {
        [SerializeField] private ModTypeIdentifier m_LifeCost = null;
        [SerializeField] private ModTypeIdentifier m_EnergyCost = null;


        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            Stat lifeCost = activeStats.GetStat(m_LifeCost.Identifier);
            Stat energyCost = activeStats.GetStat(m_EnergyCost.Identifier);

            lifeCost.AddStatModifier(new StatModifier(energyCost.Calculated, CalculationType.Flat, this));
            energyCost.AddStatModifier(new StatModifier(-1.0f, CalculationType.More, this));
        }
    }
}
