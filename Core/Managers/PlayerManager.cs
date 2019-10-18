using AdventuresUnknown.Utils;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Objects.Currencies;
using AdventuresUnknownSDK.Core.Objects.GameModes;
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
        public static string CurrentSaveFileName { get; set; }
        public static string FileExtension => Instance.FileExtensionImpl;
        public static EntityBehaviour SpaceShip => Instance.SpaceShipImpl;
        public static EntityController PlayerController { get => Instance.PlayerControllerImpl; set => Instance.PlayerControllerImpl = value; }
        public static Wallet PlayerWallet => Instance.PlayerWalletImpl;
        public static UnityEvent OnWalletDisplayChange => Instance.OnWalletDisplayChangeImpl;
        public static Currency[] WalletDisplay => Instance.WalletDisplayImpl;

        public static int Level => Instance.LevelImpl;

        protected abstract string FileExtensionImpl { get; }
        protected abstract EntityBehaviour SpaceShipImpl { get; }
        protected abstract EntityController PlayerControllerImpl { get; set; }
        protected abstract Wallet PlayerWalletImpl { get; }
        protected abstract UnityEvent OnWalletDisplayChangeImpl { get; }
        protected abstract Currency[] WalletDisplayImpl { get; }
        protected abstract int LevelImpl { get; }
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

        public static void Load(FileObject fileObject)
        {
            Instance.LoadImpl(fileObject);
        }

        public static void Delete(string file)
        {
            Instance.DeleteImpl(file);
        }

        public static void SetWalletDisplay(params string[] identifiers)
        {
            Instance.SetWalletDisplayImpl(identifiers);
        }
        
        public static FileObject[] ListSaves(GameMode gameMode)
        {
            return Instance.ListSavesImpl(gameMode);
        }

        protected abstract void SaveImpl();
        protected abstract void LoadImpl(string file);
        protected abstract void DeleteImpl(string file);
        protected abstract void LoadImpl(FileObject fileObject);
        protected abstract void SetWalletDisplayImpl(params string[] identifiers);
        protected abstract FileObject[] ListSavesImpl(GameMode gameMode);

        #endregion
    }
}
