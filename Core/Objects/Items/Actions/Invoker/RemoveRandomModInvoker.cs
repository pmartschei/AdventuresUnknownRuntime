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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Invoker/RemoveRandomModInvoker", fileName = "RemoveRandomModInvoker.asset")]
    public class RemoveRandomModInvoker : AbstractInvoker
    {

        #region Properties

        #endregion

        #region Methods
        public override void Invoke(ItemStack itemStack)
        {
            if (itemStack.ExplicitMods.Length == 0) return;

            int roll = UnityEngine.Random.Range(0, itemStack.ExplicitMods.Length);
            
            List<ValueMod> valueMods = new List<ValueMod>();
            foreach(ValueMod explicitMod in itemStack.ExplicitMods)
            {
                valueMods.Add(explicitMod);
            }
            valueMods.RemoveAt(roll);
            itemStack.ExplicitMods = valueMods.ToArray();
        }
        #endregion
    }
}
