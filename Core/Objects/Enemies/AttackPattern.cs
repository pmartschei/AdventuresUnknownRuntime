using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Enemies
{
    public abstract class AttackPattern : CoreObject
    {
        #region Properties

        #endregion

        #region Methods
        public abstract int GetAttackCount();
        public abstract int ChooseAttack(int currentAttack);
        public abstract ActiveGem GetAttack(int nr);
        #endregion
    }
}
