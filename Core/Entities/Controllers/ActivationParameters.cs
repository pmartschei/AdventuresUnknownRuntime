using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Objects.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.Controllers
{
    public class ActivationParameters
    {

        #region Properties
        public EntityController EntityController { get; set; }
        public Entity Stats { get; set; }
        public Entity FrozenStats { get; set; }
        public ActiveGem ActiveGem { get; set; }
        public ActiveGem[] SecondaryActiveGems { get; set; }
        public float Level { get; set; }
        public Muzzle[] Muzzles { get; set; }
        #endregion

        #region Methods
        public ActivationParameters()
        {
            EntityController = null;
            Stats = null;
            FrozenStats = null;
            ActiveGem = null;
            SecondaryActiveGems = null;
            Level = 0.0f;
            Muzzles = null;
        }
        #endregion
    }
}
