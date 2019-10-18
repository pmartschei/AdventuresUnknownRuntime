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
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Items/Actions/Invoker/RerollRandomModInvoker", fileName = "RerollRandomModInvoker.asset")]
    public class RerollRandomModInvoker : AbstractInvoker
    {
        [SerializeField] private ValueType m_ValueType = ValueType.Explicit;
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
            if (m_ValueType == ValueType.Implicit)
            {
                itemStackMods = itemStack.ImplicitMods;
            }
            foreach(ValueMod explicitMod in itemStackMods)
            {
                valueMods.Add(explicitMod);
            }
            switch (m_CountType)
            {
                case CountType.Single:
                    RerollSingle(ref valueMods);
                    break;
                case CountType.Value:
                    RerollCount(ref valueMods,m_Count);
                    break;
                case CountType.All:
                    RerollAll(ref valueMods);
                    break;
            }
            if (m_ValueType == ValueType.Implicit)
            {
                itemStack.ImplicitMods = valueMods.ToArray();
            }
            else
            {
                itemStack.ExplicitMods = valueMods.ToArray();
            }
        }

        private void RerollSingle(ref List<ValueMod> valueMods)
        {
            int roll = UnityEngine.Random.Range(0, valueMods.Count);

            if (valueMods[roll].Mod == null) return;

            valueMods[roll] = valueMods[roll].Mod.Roll();
        }
        private void RerollCount(ref List<ValueMod> valueMods,int count)
        {
            int length = valueMods.Count;
            for (int i = 0; i < valueMods.Count; i++)
            {
                float chance = count / length;

                if (UnityEngine.Random.Range(0.0f,1.0f) >= chance)
                {
                    if (valueMods[i].Mod == null) continue;
                    valueMods[i] = valueMods[i].Mod.Roll();
                    count--;
                    if (count == 0) break;
                }
                length--;
            }
        }
        private void RerollAll(ref List<ValueMod> valueMods)
        {
            for(int i = 0; i < valueMods.Count; i++)
            {
                if (valueMods[i].Mod == null) continue;

                valueMods[i] = valueMods[i].Mod.Roll();
            }
        }
        #endregion
    }
}
