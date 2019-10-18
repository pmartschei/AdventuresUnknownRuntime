using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items.Interfaces;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Tags;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;

namespace AdventuresUnknownSDK.Core.Objects.Items.Actions.Invoker
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Invoker/AddRandomModInvoker", fileName = "AddRandomModInvoker.asset")]
    public class AddRandomModInvoker : AbstractInvoker
    {

        #region Properties

        #endregion

        #region Methods
        public override void Invoke(ItemStack itemStack)
        {
            Mod[] availableMods = ModifierManager.GetModifiersForDomainAndTag(1, itemStack.Item.Tags.ToArray());
            List<ValueMod> valueMods = new List<ValueMod>();
            foreach(ValueMod explicitMod in itemStack.ExplicitMods)
            {
                valueMods.Add(explicitMod);
            }
            availableMods = ModUtils.Filter(availableMods, itemStack.ItemLevel);
            availableMods = ModUtils.Filter(availableMods, valueMods.ToArray());
            ValueMod valueMod = ModUtils.Roll(availableMods, itemStack.Item.Tags.ToArray());
            if (valueMod == null) return;
            valueMods.Add(valueMod);
            itemStack.ExplicitMods = valueMods.ToArray();
        }
        #endregion
    }
}
