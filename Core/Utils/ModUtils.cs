using AdventuresUnknownSDK.Core.Objects.Mods;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;

namespace AdventuresUnknownSDK.Core.Utils
{
    public static class ModUtils
    {


        #region Properties

        #endregion

        #region Methods
        public static ValueMod Roll(Mod[] mods,params string[] tags)
        {
            int weight = 0;
            Mod[] rollableMods = Filter(mods, tags);
            foreach (Mod availableMod in rollableMods)
            {
                weight += availableMod.GetSumOfTags(tags);
            }
            if (weight == 0) return null;
            int roll = UnityEngine.Random.Range(1, weight);
            int modIndex = 0;
            int sumOfTags = rollableMods[modIndex].GetSumOfTags(tags);
            while (roll > sumOfTags)
            {
                roll -= sumOfTags;
                modIndex++;
                if (modIndex >= rollableMods.Length) return null;
                sumOfTags = rollableMods[modIndex].GetSumOfTags(tags);
            }
            return rollableMods[modIndex].Roll();
        }

        public static Mod[] Filter(Mod[] mods,params ValueMod[] valueMods)
        {
            List<string> bannedModGroups = new List<string>();
            foreach (ValueMod explicitMod in valueMods)
            {
                foreach (string modGroup in explicitMod.Mod.ModGroups)
                {
                    if (modGroup.Equals(string.Empty)) continue;
                    if (bannedModGroups.Contains(modGroup)) continue;
                    bannedModGroups.Add(modGroup);
                }
            }
            return mods.Where((mod) => {
                foreach(ValueMod valueMod in valueMods)
                {
                    if (valueMod.BasicModBase == mod.ModBase) return false;
                }
                return (mod.ModGroups.Intersect(bannedModGroups).Count() == 0);
            }).ToArray();
        }

        public static Mod[] Filter(Mod[] mods,int maxLevel,int minLevel = 0)
        {
            return mods.Where((mod) => { return mod.RequiredLevel <= maxLevel && mod.RequiredLevel >= minLevel; }).ToArray();
        }
        public static Mod[] Filter(Mod[] mods, params string[] tags)
        {
            List<Mod> rollableMods = new List<Mod>();
            foreach (Mod availableMod in mods)
            {
                int sum = availableMod.GetSumOfTags(tags);
                if (sum > 0)
                {
                    rollableMods.Add(availableMod);
                }
            }
            return rollableMods.ToArray();
        }
        #endregion
    }
}
