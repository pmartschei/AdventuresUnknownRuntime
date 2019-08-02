using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Entities
{
    [Serializable]
    public class Entity
    {
        [NonSerialized] private GameObject m_GameObject = null;
        [NonSerialized] private List<IActiveStat> m_IActiveStats = new List<IActiveStat>();
        [NonSerialized] private Dictionary<string, Stat> m_Stats = new Dictionary<string, Stat>();
        [NonSerialized] private List<ModType> m_ModTypes = new List<ModType>();

        [NonSerialized] private Dictionary<Stat, List<BaseAction>> m_NotifyCollection = new Dictionary<Stat, List<BaseAction>>();
        [NonSerialized] private Dictionary<object, TimerObject> m_TimerObjects = new Dictionary<object, TimerObject>();
        [NonSerialized] private Dictionary<object, object> m_ObjectDictionary = new Dictionary<object, object>();

        [SerializeField] private bool m_CanTick = true;
        [SerializeField] private EntityDescription m_Description = new EntityDescription();
        [HideInInspector] [SerializeField] private bool m_StatsChanged = false;

        private UnityEvent m_OnStatsChangedEvent = new UnityEvent();

        #region Properties
        public Stat[] Stats
        {
            get
            {
                RecalculateStats();
                return m_Stats.Values.ToArray();
            }
        }

        public bool StatsChanged
        {
            get
            {
                if (m_StatsChanged) return true;
                foreach (IActiveStat activeStat in m_IActiveStats)
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

        public bool CanTick { get => m_CanTick; set => m_CanTick = value; }
        public EntityDescription Description { get => m_Description; set => m_Description = value; }
        public GameObject GameObject { get => m_GameObject; set => m_GameObject = value; }
        public bool IsDead { get; set; }

        #endregion
        #region Methods

        public void Start()
        {
            Notify(ActionTypeManager.Spawn);
        }
        public void Reset()
        {
            m_IActiveStats.Clear();
            m_Stats.Clear();
            m_ModTypes.Clear();
            m_NotifyCollection.Clear();
            m_TimerObjects.Clear();
            m_ObjectDictionary.Clear();
            m_StatsChanged = false;
        }
        public void Tick(float time)
        {
            if (CanTick)
            {
                UpdateTimers(time);
                TickStats(time);
            }
        }

        private void RecalculateStats()
        {
            if (StatsChanged)
            {
                StatsChanged = false;
                List<string> keys = m_Stats.Keys.ToList();
                Dictionary<string, float> currentValues = new Dictionary<string, float>();
                for (int i = 0; i < keys.Count; i++)
                {
                    Stat stat = GetStat(keys[i]);
                    currentValues.Add(keys[i], stat.Current);
                }
                m_ModTypes.Clear();
                m_NotifyCollection.Clear();
                m_Stats.Clear();
                foreach (IActiveStat activeStat in m_IActiveStats)
                {
                    activeStat.Initialize(this);
                }
                foreach(var pair in currentValues)
                {
                    GetStat(pair.Key).Current = pair.Value;
                }
            }
        }

        private void UpdateTimers(float time)
        {
            List<TimerObject> timerList = m_TimerObjects.Values.ToList();

            for (int i = 0; i < timerList.Count; i++)
            {
                TimerObject timerObject = timerList[i];
                timerObject.Update(time);
                if (timerObject.IsFinished())
                {
                    m_TimerObjects.Remove(timerObject.Source);
                    timerObject.Callback(this);
                }
            }
        }

        private void TickStats(float time)
        {
            Notify(ActionTypeManager.Tick, new TickContext(time));
            Stat[] stats = m_Stats.Values.ToArray();
            foreach (Stat stat in stats)
            {
                if (stat.StatChanged)
                {
                    List<BaseAction> baseActions = GetNotifyStatList(stat);
                    if (baseActions != null)
                    {
                        foreach (BaseAction baseAction in baseActions)
                        {
                            baseAction.Notify(this, null);
                        }
                    }
                    stat.StatChanged = false;
                }
            }
        }
        private List<BaseAction> GetNotifyStatList(Stat stat)
        {
            if (!m_NotifyCollection.ContainsKey(stat)) return null;
            return m_NotifyCollection[stat];
        }
        public void NotifyOnStatChange(Stat stat, BaseAction baseAction)
        {
            List<BaseAction> list = new List<BaseAction>();
            if (!m_NotifyCollection.ContainsKey(stat))
            {
                m_NotifyCollection.Add(stat, list);
            }
            else
            {
                list = m_NotifyCollection[stat];
            }
            if (!list.Contains(baseAction))
                list.Add(baseAction);
        }
        public void Notify(ActionType actionType)
        {
            Notify(actionType, null);
        }
        public void Notify(ActionType actionType, ActionContext actionContext)
        {
            RecalculateStats();
            List<BaseAction> baseActions = ModActionManager.GetActions(actionType, m_ModTypes.ToArray());
            foreach (BaseAction baseAction in baseActions)
            {
                baseAction.Notify(this, actionContext);
            }
        }
        public void AddTimer(TimerObject timerObject)
        {
            if (timerObject.Source == null) return;
            if (timerObject.Duration <= 0.0f) return;
            if (GetTimer(timerObject.Source) != null) return;
            m_TimerObjects.Add(timerObject.Source, timerObject);
        }
        public TimerObject GetTimer(object source)
        {
            if (!m_TimerObjects.ContainsKey(source)) return null;
            return m_TimerObjects[source];
        }
        public bool AddObject(object source, object obj)
        {
            return AddObject(source, obj, false);
        }
        public bool AddObject(object source, object obj, bool replace)
        {
            if (!replace && m_ObjectDictionary.ContainsKey(source)) return false;
            m_ObjectDictionary.Add(source, obj);
            return true;
        }
        public object GetObject(object source)
        {
            if (!m_ObjectDictionary.ContainsKey(source)) return null;
            return m_ObjectDictionary[source];
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
            RecalculateStats();
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
                    m_ModTypes.Add(newStat.ModType);

                    List<int> actionValues = ModActionManager.GetAllRegisteredActionValues();
                    foreach (int actionValue in actionValues)
                    {
                        List<BaseAction> baseActions = ModActionManager.GetActions(actionValue, newStat.ModType);
                        foreach(BaseAction baseAction in baseActions)
                        {
                            baseAction.Initialize(this);
                        }
                    }
                }
                return newStat;
            }
            return m_Stats[modTypeIdentifier] as Stat;
        }
        

        public void CopyFrom(Entity entity)
        {
            if (entity == null) return;
            GameObject = entity.GameObject;
            entity.RecalculateStats();
            foreach (var activeStat in entity.m_IActiveStats)
            {
                m_IActiveStats.Add(activeStat);
            }

            foreach (var pair in entity.m_Stats)
            {
                m_Stats.Add(pair.Key, pair.Value.Copy());
                m_ModTypes.Add(pair.Value.ModType);
            }

            foreach (var pair in entity.m_NotifyCollection)
            {
                m_NotifyCollection.Add(GetStat(pair.Key.ModTypeIdentifier), pair.Value);
            }
        }

        public void AddFrom(Entity entity)
        {
            if (entity == null) return;
            RecalculateStats();
            entity.RecalculateStats();
            foreach (var activeStat in entity.m_IActiveStats)
            {
                m_IActiveStats.Add(activeStat);
            }
            foreach (var pair in entity.m_Stats)
            {
                if (m_Stats.ContainsKey(pair.Key))
                {
                    Stat stat = m_Stats[pair.Key];
                    stat += pair.Value.Copy();
                    continue;
                }
                m_Stats.Add(pair.Key, pair.Value.Copy());
                m_ModTypes.Add(pair.Value.ModType);
            }
            foreach (var pair in entity.m_NotifyCollection)
            {
                Stat stat = GetStat(pair.Key.ModTypeIdentifier);
                if (m_NotifyCollection.ContainsKey(stat))
                {
                    List<BaseAction> list = m_NotifyCollection[stat];
                    foreach (var item in pair.Value)
                    {
                        if (list.Contains(item)) continue;
                        list.Add(item);
                    }
                    continue;
                }
                m_NotifyCollection.Add(stat, pair.Value);
            }
        }

       
        #endregion
    }
}