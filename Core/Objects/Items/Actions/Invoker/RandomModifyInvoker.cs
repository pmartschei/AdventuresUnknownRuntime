using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items.Interfaces;
using AdventuresUnknownSDK.Core.Objects.Mods;
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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Invoker/RandomModifyInvoker", fileName = "RandomModifyInvoker.asset")]
    public class RandomModifyInvoker : AbstractInvoker
    {
        [SerializeField] private AnimationCurve m_ExplicitCount = null;

        #region Properties

        #endregion

        #region Methods
        public override void Invoke(ItemStack itemStack)
        {
            int count = (int)m_ExplicitCount.Evaluate(UnityEngine.Random.Range(0.0f, 1.0f));

            Mod[] availableMods = ModifierManager.GetModifiersForDomainAndTag(1, itemStack.Item.Tags.ToArray());
            availableMods = ModUtils.Filter(availableMods,itemStack.ItemLevel);
            List<ValueMod> valueMods = new List<ValueMod>();
            for(int i = 0; i < count; i++)
            {
                availableMods = ModUtils.Filter(availableMods, valueMods.ToArray());
                ValueMod valueMod = ModUtils.Roll(availableMods, itemStack.Item.Tags.ToArray());
                if (valueMod != null)
                {
                    valueMods.Add(valueMod);
                }
            }
            itemStack.ExplicitMods = valueMods.ToArray();
        }
        #endregion
    }
}
