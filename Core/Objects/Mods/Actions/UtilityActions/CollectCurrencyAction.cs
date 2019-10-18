using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.DropTables;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions.UtilityActions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/Utility/CollectCurrencyAction", fileName = "CollectCurrencyAction.asset")]
    public class CollectCurrencyAction : MultipleBaseAction
    {
        [SerializeField] private ModTypeIdentifier m_Source = null;
        [SerializeField] private CurrencyIdentifier m_Currency = null;
        [SerializeField] private bool m_UseMagicFind = false;
        [SerializeField] private ModTypeIdentifier m_MagicFind = null;

        #region Properties

        #endregion

        #region Methods

        #endregion

        public override void Notify(Entity activeStats, ActionContext actionContext)
        {
            float gold = activeStats.GetStat(m_Source.Identifier).Calculated;
            if (gold == 0.0f) return;

            if (m_UseMagicFind)
            {
                gold *= (1.0f + PlayerManager.PlayerController.Entity.GetStat(m_MagicFind.Identifier).Calculated);
            }

            PlayerManager.PlayerWallet.AddValue(m_Currency.Identifier, (long)gold);
        }
    }
}
