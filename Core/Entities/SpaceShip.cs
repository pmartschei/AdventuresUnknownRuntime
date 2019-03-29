using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using static AdventuresUnknownSDK.Core.Objects.Inventories.ItemStack;

namespace AdventuresUnknownSDK.Core.Entities
{
    public class SpaceShip : MonoBehaviour,IActiveStat
    {
        
        [SerializeField] private bool m_IsDead = false;
        [SerializeField] private ActiveStats m_ActiveStats = new ActiveStats();
        
        #region Properties

        public bool IsDead { get => m_IsDead; set => m_IsDead = value; }
        public ActiveStats ActiveStats { get => m_ActiveStats; }
        public bool StatsChanged { get => m_ActiveStats.StatsChanged; set => m_ActiveStats.StatsChanged = value; }

        public Stat[] Stats => m_ActiveStats.Stats;
        public Stat[] RawStats => m_ActiveStats.RawStats;
        #endregion

        #region Methods
        private void Update()
        {
            //for(int i=0;i<10000;i++)
            //m_ActiveStats.Tick(Time.deltaTime);
        }
        public void AddActiveStat(IActiveStat activeStat)
        {
            m_ActiveStats.AddActiveStat(activeStat);
        }

        public Stat GetStat(string modTypeIdentifier)
        {
            return m_ActiveStats.GetStat(modTypeIdentifier);
        }

        public void RemoveActiveStat(IActiveStat activeStat)
        {
            m_ActiveStats.RemoveActiveStat(activeStat);
        }
        #endregion
    }
}
