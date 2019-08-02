using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Enemies
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Enemies/AttackPatterns/SingleAttackPattern", fileName = "SingleAttackPattern.asset")]
    public class SingleAttackPattern : AttackPattern
    {
        [SerializeField] private ActiveGem m_ActiveItemStack = null;

        #region Properties

        #endregion

        #region Methods
        public override int GetAttackCount()
        {
            return 1;
        }
        public override int ChooseAttack(int currentAttack)
        {
            return 0;
        }
        public override ActiveGem GetAttack(int nr)
        {
            return m_ActiveItemStack;
        }
        #endregion
    }
}
