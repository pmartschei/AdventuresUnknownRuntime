using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects;
using AdventuresUnknownSDK.Core.Utils.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    public abstract class IPlayerData : CoreObject
    {
        public abstract void Reset();
        public abstract void Load();
        public override void Initialize()
        {
            base.Initialize();
            Reset();
        }
    }
}
