using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items.Interfaces;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;

namespace AdventuresUnknownSDK.Core.Objects.Items.Actions.Invoker
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Invoker/AddRandomMod", fileName = "AddRandomModInvoker.asset")]
    public class AddRandomModInvoker : AbstractInvoker
    {


        #region Properties

        #endregion

        #region Methods
        public override void Invoke(ItemStack itemStack)
        {
            string[] tags = new string[itemStack.Item.Tags.Count];
            int i = 0;
            foreach (Tag tag in itemStack.Item.Tags)
            {
                tags[i] = tag.Identifier;
                i++;
            }
            Mod[] availableMods = ModifierManager.GetModifiersForDomainAndTag(1, tags);
            int weight = 0;
            foreach (Mod availableMod in availableMods)
            {
                weight += availableMod.GetSumOfTags(tags);
            }
            if (weight == 0) return;
            int roll = UnityEngine.Random.Range(0, weight);
            int modIndex = 0;
            int sumOfTags = availableMods[modIndex].GetSumOfTags(tags);
            while (roll < sumOfTags)
            {
                roll -= sumOfTags;
                modIndex++;
                if (modIndex >= availableMods.Length) break;
                sumOfTags = availableMods[modIndex].GetSumOfTags(tags);
            }
            itemStack.ExplicitMods = new ItemStack.ValueMod[] { availableMods[modIndex - 1].Roll() };
        }
        #endregion
    }
}
