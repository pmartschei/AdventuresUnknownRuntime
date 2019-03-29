using AdventuresUnknownSDK.Core.Datas;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class PlayerManager : SingletonBehaviour<PlayerManager>
    {

        #region Properties
        public static SpaceShip SpaceShip => Instance.SpaceShipImpl;

        protected abstract SpaceShip SpaceShipImpl { get; }
        #endregion

        #region Methods
        public static void Save()
        {
            Instance.SaveImpl();
        }

        public static void Load(string file)
        {
            Instance.LoadImpl(file);
        }
        
        protected abstract void SaveImpl();
        protected abstract void LoadImpl(string file);
        #endregion
    }
}
