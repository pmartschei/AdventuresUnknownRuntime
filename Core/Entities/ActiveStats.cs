using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities
{
    [Serializable]
    public class ActiveStats : IActiveStat
    {
        [NonSerialized] private List<IActiveStat> m_IActiveStats = new List<IActiveStat>();
        [NonSerialized] private Dictionary<string, Stat> m_Stats = new Dictionary<string, Stat>();
        [NonSerialized] private Dictionary<string, Stat> m_RawStats = new Dictionary<string, Stat>();

        private bool m_StatsChanged = false;
        #region Properties
        public bool ValueChanged
        {
            get => false;
            set { }
        }

        public Stat[] Stats
        {
            get
            {
                RecalculateStats();
                Stat[] res = new Stat[m_Stats.Count];
                m_Stats.Values.CopyTo(res, 0);
                return res;
            }
        }

        public Stat[] RawStats
        {
            get
            {
                RecalculateStats();
                Stat[] res = new Stat[m_RawStats.Count];
                m_RawStats.Values.CopyTo(res, 0);
                return res;
            }
        }

        public bool StatsChanged
        {
            get
            {
                if (m_StatsChanged) return true;
                foreach(IActiveStat activeStat in m_IActiveStats)
                {
                    if (activeStat.StatsChanged) return true;
                }
                return false;
            }
            set
            {
                m_StatsChanged = value;
                foreach (IActiveStat activeStat in m_IActiveStats)
                {
                    activeStat.StatsChanged = value;
                }
            }
        }

        #endregion
        #region Methods

        public void Tick(float time)
        {
            TickStats(m_Stats, time);
        }

        private void RecalculateStats()
        {
            if (StatsChanged)
            {
                m_Stats.Clear();
                m_RawStats.Clear();
                foreach (IActiveStat activeStat in m_IActiveStats)
                {
                    Stat[] stats = activeStat.RawStats;
                    foreach (Stat stat in stats)
                    {
                        if (m_Stats.ContainsKey(stat.ModTypeIdentifier))
                        {
                            Stat hashedStat = m_Stats[stat.ModTypeIdentifier];
                            hashedStat += stat;
                        }
                        else
                        {
                            m_Stats.Add(stat.ModTypeIdentifier, stat);
                            m_RawStats.Add(stat.ModTypeIdentifier, stat);
                        }
                    }
                }
                CalculateStats(m_Stats);
                StatsChanged = false;
            }
        }

        private void TickStats(Dictionary<string, Stat> dictionaryStats, float time)
        {
            List<Stat> stats = dictionaryStats.Values.ToList();
            stats.Sort((x, y) => { return y.ModType.Priority - x.ModType.Priority; });
            foreach (Stat stat in stats)
            {
                stat.ModType.OnTick(this,time);
            }
            CalculateStats(dictionaryStats);
        }

        private void CalculateStats(Dictionary<string, Stat> dictionaryStats)
        {
            List<Stat> stats = dictionaryStats.Values.ToList();
            stats.Sort((x, y) => { return y.ModType.Priority - x.ModType.Priority; });
            foreach(Stat stat in stats)
            {
                stat.ModType.Calculate(this);
            }
        }

        public virtual void AddActiveStat(IActiveStat activeStat)
        {
            if (m_IActiveStats.Contains(activeStat)) return;
            m_IActiveStats.Add(activeStat);
            StatsChanged = true;
        }
        public virtual void RemoveActiveStat(IActiveStat activeStat)
        {
            if (m_IActiveStats.Remove(activeStat))
                StatsChanged = true;
        }

        public Stat GetStat(string modTypeIdentifier)
        {
            if (!m_Stats.ContainsKey(modTypeIdentifier))
            {
                Stat newStat = new Stat(ObjectsManager.FindObjectByIdentifier<ModType>(modTypeIdentifier));
                if (newStat.ModType == null)
                {
                    GameConsole.LogWarningFormat("Could not get Stat with Identifier {0}", modTypeIdentifier);
                }
                else
                {
                    m_Stats.Add(modTypeIdentifier, newStat);
                }
                return newStat;
            }
            return m_Stats[modTypeIdentifier] as Stat;
        }
        #endregion
    }
}