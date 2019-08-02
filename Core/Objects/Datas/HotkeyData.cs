using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Utils.Events;
using System;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Objects.Datas
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Datas/HotkeyData", fileName = "HotkeyData.asset")]
    public class HotkeyData : IPlayerData
    {



        #region Properties

        #endregion

        #region Methods
        //public override bool OnAfterDeserialize()
        //{
        //    ContextData contextData = FindScriptableObject<ContextData>();
        //    if (!contextData) return false;
        //    contextData.ShipName = this.ShipName;
        //    contextData.Level = this.Level;
        //    contextData.Experience = this.Experience;
        //    contextData.PlayTime = this.PlayTime;
        //    return true;
        //}
        #endregion
        public override void Load()
        {

        }

        public override void Reset()
        {

        }
    }
}
