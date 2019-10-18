using AdventuresUnknownSDK.Core.Entities.Weapons;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces
{
    public interface IMuzzleComponentController
    {


        #region Properties
        Muzzle[] Muzzles { get; set; }
        #endregion

        #region Methods
        Muzzle FindMuzzle(string name);
        Muzzle[] FindMuzzles(params string[] names);
        #endregion
    }
}
