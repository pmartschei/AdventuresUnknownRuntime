using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Utils.UnityEvents;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.UI
{
    public class UIDisplayPlayerStat : MonoBehaviour
    {
        [SerializeField] private ModTypeIdentifier m_ModType = null;
        [SerializeField] private StatEvent m_OnStatChangeEvent = new StatEvent();

        private Stat m_LastStat = null;

        #region Properties

        #endregion

        #region Methods
        private void Update()
        {
            EntityBehaviour entityBehaviour = PlayerManager.SpaceShip;
            Stat stat = entityBehaviour.Entity.GetStat(m_ModType.Identifier);

            m_OnStatChangeEvent.Invoke(stat);
            return;

            if (m_LastStat == null || !m_LastStat.Equals(stat))
            {
                m_LastStat = stat.Copy();
                if (m_OnStatChangeEvent != null)
                    m_OnStatChangeEvent.Invoke(m_LastStat);
            }
        }
        #endregion
    }
}
