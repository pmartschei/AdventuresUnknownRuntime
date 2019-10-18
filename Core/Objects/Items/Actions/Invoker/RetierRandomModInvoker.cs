using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items.Interfaces;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Invoker/RetierRandomModInvoker", fileName = "RetierRandomModInvoker.asset")]
    public class RetierRandomModInvoker : AbstractInvoker
    {
        [SerializeField] private CountType m_CountType = CountType.Single;
        [Tooltip("Count of mods changed when using CountType.Value")]
        [SerializeField] private int m_Count = 0;

        public enum CountType
        {
            Single,
            Value,
            All,
        }
        #region Properties

        #endregion

        #region Methods
        public override void Invoke(ItemStack itemStack)
        {
            List<ValueMod> valueMods = new List<ValueMod>();
            ValueMod[] itemStackMods = itemStack.ExplicitMods;
            
            Mod[] availableMods = ModifierManager.GetModifiersForDomainAndTag(1, itemStack.Item.Tags.ToArray());
            availableMods = ModUtils.Filter(availableMods, itemStack.ItemLevel);
            foreach (ValueMod explicitMod in itemStackMods)
            {
                valueMods.Add(explicitMod);
            }
            switch (m_CountType)
            {
                case CountType.Single:
                    RetierSingle(ref valueMods,availableMods);
                    break;
                case CountType.Value:
                    RetierCount(ref valueMods,m_Count, availableMods);
                    break;
                case CountType.All:
                    RetierAll(ref valueMods, availableMods);
                    break;
            }
            itemStack.ExplicitMods = valueMods.ToArray();
        }

        private void RetierSingle(ref List<ValueMod> valueMods, Mod[] availableMods)
        {
            int firstRoll = UnityEngine.Random.Range(0, valueMods.Count);
            RetierSingleAt(ref valueMods, availableMods, firstRoll);
        }

        private void RetierSingleAt(ref List<ValueMod> valueMods,Mod[] availableMods,int index)
        {
            Mod otherMod = valueMods[index].Mod;
            Mod[] list = availableMods.Where((mod) => { return mod.ModGroups.Union(otherMod.ModGroups).Count() == mod.ModGroups.Length; }).ToArray();

            int roll = UnityEngine.Random.Range(0, list.Length);

            valueMods[index] = list[roll].Roll();
        }
        private void RetierCount(ref List<ValueMod> valueMods,int count, Mod[] availableMods)
        {
            int length = valueMods.Count;
            for (int i = 0; i < valueMods.Count; i++)
            {
                float chance = count / length;

                if (UnityEngine.Random.Range(0.0f,1.0f) >= chance)
                {
                    RetierSingleAt(ref valueMods, availableMods, i);
                    count--;
                    if (count == 0) break;
                }
                length--;
            }
        }
        private void RetierAll(ref List<ValueMod> valueMods, Mod[] availableMods)
        {
            for(int i = 0; i < valueMods.Count; i++)
            {
                RetierSingleAt(ref valueMods, availableMods, i);
            }
        }
        #endregion
    }
}
