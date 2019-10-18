using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.DropTables;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.UtilityActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/DropOnDeathAction", fileName = "DropOnDeathAction.asset")]
    public class DropOnDeathAction : MultipleBaseAction
    {


        #region Properties

        #endregion

        #region Methods

        #endregion

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            if (activeStats.GetStat(ModType.Identifier).Calculated == 0.0f) return;
            DropRate[] dropRates = DropManager.GetDropRates(activeStats.Description.Enemy);

            List<ItemStack> rolledItemStacks = new List<ItemStack>();

            foreach(DropRate dropRate in dropRates)
            {
                //TODO maxlevel from enemy? and rate multiplier = magic find
                rolledItemStacks.AddRange(dropRate.Roll((int)activeStats.GetStat("core.modtypes.ship.level").Calculated,
                    1.0f + PlayerManager.PlayerController.Entity.GetStat("core.modtypes.utility.magicfind").Calculated));
            }

            foreach(ItemStack rolledItemStack in rolledItemStacks)
            {
                ModifierManager.ModifyItemStack(rolledItemStack);
                DropManager.GenerateDrop(rolledItemStack, activeStats.EntityBehaviour.transform.position);
            }
        }
    }
}
