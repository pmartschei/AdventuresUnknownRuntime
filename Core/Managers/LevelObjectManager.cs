using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class LevelObjectManager : SingletonBehaviour<LevelObjectManager>
    {
        public static GameObject Wall => Instance.WallImpl;
        protected abstract GameObject WallImpl { get; }
    }
}