using AdventuresUnknownSDK.Core.Entities.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces
{
    public interface IAttackController
    {


        #region Properties

        #endregion

        #region Methods
        void Attack(EntityController origin, int index, params Muzzle[] muzzles);
        void Attack(int index, params Muzzle[] muzzles);
        Entity GetEntity(int index);
        bool IsNearTarget(int index, float multiplier = 0.9f);
        bool HasCooldown(EntityController origin, int index);
        bool HasCooldown(int index);
        bool IsAttackValid(int index);
        bool RequiresTarget(int index);
        float GetAttackPriority(int index);
        float GetAttackMaxDistance(int index);
        #endregion
    }
}
