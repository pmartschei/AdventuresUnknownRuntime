﻿using AdventuresUnknownSDK.Core.Log;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Effects;
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
    public class Entity : IActiveStat
    {
        [NonSerialized] private EntityBehaviour m_EntityBehaviour = null;
        [NonSerialized] private Dictionary<string, Stat> m_Stats = new Dictionary<string, Stat>();

        [NonSerialized] private Dictionary<object, TimerObject> m_TimerObjects = new Dictionary<object, TimerObject>();
        [NonSerialized] private Dictionary<object, object> m_ObjectDictionary = new Dictionary<object, object>();

        [SerializeField] private bool m_CanTick = true;
        [SerializeField] private EntityDescription m_Description = new EntityDescription();
        [HideInInspector] [SerializeField] private bool m_StatsChanged = false;

        [NonSerialized] private List<Entity> m_RegisteredEntities = new List<Entity>();
        [NonSerialized] private Dictionary<string, EffectContext> m_Effects = new Dictionary<string, EffectContext>();

        private Dictionary<string, List<Entity>> m_MinionsDictionary = new Dictionary<string, List<Entity>>();

        #region Properties
        public Stat[] Stats
        {
            get
            {
                return m_Stats.Values.ToArray();
            }
        }

        public bool CanTick { get => m_CanTick; set => m_CanTick = value; }
        public EntityDescription Description { get => m_Description; set => m_Description = value; }
        public EntityBehaviour EntityBehaviour { get => m_EntityBehaviour; set => m_EntityBehaviour = value; }
        public bool IsDead { get; set; }
        public List<Entity> RegisteredEntities { get => m_RegisteredEntities; }
        public EffectContext[] Effects { get => m_Effects.Values.ToArray(); }

        #endregion
        #region Methods

        public void Start()
        {
            Notify(ActionTypeManager.Spawn);
        }
        public void Reset()
        {
            m_Stats.Clear();
            m_TimerObjects.Clear();
            m_ObjectDictionary.Clear();
            m_MinionsDictionary.Clear();
            m_Effects.Clear();
            m_StatsChanged = false;
        }
        public void Tick(float time)
        {
            if (CanTick)
            {
                UpdateTimers(time);
                TickEffects(time);
                TickStats(time);
            }
        }

        private void TickEffects(float time)
        {
            EffectContext[] effectContexts = m_Effects.Values.ToArray();
            for (int i = 0; i < effectContexts.Length; i++)
            {
                effectContexts[i].Update(time);
                if (effectContexts[i].IsGone())
                {
                    RemoveEffect(effectContexts[i].Effect.Identifier);
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
        }
        public void Notify(ActionType actionType)
        {
            Notify(actionType, null);
        }
        public void Notify(ActionType actionType, ActionContext actionContext)
        {
            List<BaseAction> baseActions = ModActionManager.GetActions(actionType, null);
            foreach (BaseAction baseAction in baseActions)
            {
                baseAction.PreNotify(this, actionContext);
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
            object value;
            if (m_ObjectDictionary.TryGetValue(source, out value))
            {
                if (!replace) return false;
                m_ObjectDictionary.Remove(source);
            }
            m_ObjectDictionary.Add(source, obj);
            return true;
        }
        public object GetObject(object source)
        {
            object value;
            if (m_ObjectDictionary.TryGetValue(source, out value))
            {
                return value;
            }
            return null;
        }

        public object RemoveObject(object source)
        {
            object value;
            if (m_ObjectDictionary.TryGetValue(source, out value))
            {
                m_ObjectDictionary.Remove(source);
            }
            return value;
        }

        public T GetObject<T>(object source)
        {
            object obj = GetObject(source);

            if (obj != null && obj is T)
            {
                return (T)(obj);
            }
            return default(T);
        }

        public virtual void RemoveAllModifiersBySource(object source)
        {
            Stat[] stats = m_Stats.Values.ToArray();
            foreach (Stat stat in stats)
            {
                stat.RemoveStatModifiersBySource(source);
            }
        }
        public void ApplyEffect(string effectIdentifier, float duration, float value)
        {
            EffectContext context;
            if (!m_Effects.TryGetValue(effectIdentifier, out context))
            {
                Effect effect = ObjectsManager.FindObjectByIdentifier<Effect>(effectIdentifier);

                if (effect == null)
                {
                    GameConsole.LogFormat("Effect {0} not found", effectIdentifier);
                    return;
                }
                context = new EffectContext(this, effect);
                m_Effects.Add(effectIdentifier, context);
            }
            context.AddEffect(duration, value);
        }

        public void RemoveEffect(string effectIdentifier)
        {
            EffectContext context;
            if (m_Effects.TryGetValue(effectIdentifier, out context))
            {
                context.Remove();
                m_Effects.Remove(effectIdentifier);
            }
        }

        public void AddMinion(string modTypeIdentifier, Entity entity)
        {
            List<Entity> minionList;
            if (!m_MinionsDictionary.TryGetValue(modTypeIdentifier, out minionList))
            {
                minionList = new List<Entity>();
                m_MinionsDictionary.Add(modTypeIdentifier, minionList);
            }
            if (minionList.Contains(entity)) return;
            GetStat(modTypeIdentifier).Current++;
            minionList.Add(entity);
        }

        public void RemoveMinion(string modTypeIdentifier, Entity entity)
        {
            List<Entity> minionList;
            if (m_MinionsDictionary.TryGetValue(modTypeIdentifier, out minionList))
            {
                if (minionList.Remove(entity))
                {
                    GetStat(modTypeIdentifier).Current--;
                }
            }
        }

        public List<Entity> GetMinions(string modTypeIdentifier)
        {
            List<Entity> minionList;
            m_MinionsDictionary.TryGetValue(modTypeIdentifier, out minionList);
            return minionList;
        }

        public List<Entity> GetAllMinions()
        {
            List<Entity> minionList = new List<Entity>();
            foreach (var value in m_MinionsDictionary.Values)
            {
                minionList.AddRange(value);
            }
            return minionList.Distinct().ToList();
        }

        public Stat GetStat(string modTypeIdentifier)
        {
            Stat foundStat;
            m_Stats.TryGetValue(modTypeIdentifier, out foundStat);
            if (foundStat != null) return foundStat;

            Stat newStat = new Stat(ObjectsManager.FindObjectByIdentifier<ModType>(modTypeIdentifier));
            if (newStat.ModType == null)
            {
                GameConsole.LogWarningFormat("Could not get Stat with Identifier {0}", modTypeIdentifier);
                Debug.LogWarningFormat(this.EntityBehaviour, "Could not get Stat with Identifier{0}", modTypeIdentifier);
            }
            else
            {
                m_Stats.Add(modTypeIdentifier, newStat);
                
                List<BaseAction> actions = ModActionManager.GetActions(0, newStat.ModType);
                foreach (BaseAction action in actions)
                {
                    action.Notify(this, null);
                }
                foreach (Entity entity in RegisteredEntities)
                {
                    Stat registeredStat = entity.GetStat(newStat.ModTypeIdentifier);
                    registeredStat.AddStatModifier(new StatModifierByStat(newStat, CalculationType.Flat, this));
                    registeredStat.AddStatModifier(new StatModifierByStat(newStat, CalculationType.Increased, this));
                    registeredStat.AddStatModifier(new StatModifierByStat(newStat, CalculationType.More, this));
                    registeredStat.AddStatModifier(new StatModifierByStat(newStat, CalculationType.FlatExtra, this));
                }
            }
            return newStat;
        }



        public void CopyFrom(Entity entity)
        {
            if (entity == null) return;
            EntityBehaviour = entity.EntityBehaviour;

            foreach (var pair in entity.m_Stats)
            {
                m_Stats.Add(pair.Key, pair.Value.Copy());
            }
        }

        public void AddFrom(Entity entity)
        {
            if (entity == null) return;
            foreach (var pair in entity.m_Stats)
            {
                if (m_Stats.ContainsKey(pair.Key))
                {
                    Stat stat = m_Stats[pair.Key];
                    stat += pair.Value.Copy();
                    continue;
                }
                m_Stats.Add(pair.Key, pair.Value.Copy());
            }
        }

        private void AddModifiers(Entity entity)
        {
            if (entity == null) return;
            RemoveAllModifiers(entity);

            foreach (Stat stat in Stats)
            {
                Stat entityStat = entity.GetStat(stat.ModTypeIdentifier);
                entityStat.AddStatModifier(new StatModifierByStat(stat, CalculationType.Flat, this));
                entityStat.AddStatModifier(new StatModifierByStat(stat, CalculationType.Increased, this));
                entityStat.AddStatModifier(new StatModifierByStat(stat, CalculationType.More, this));
                entityStat.AddStatModifier(new StatModifierByStat(stat, CalculationType.FlatExtra, this));
            }
        }


        private void RemoveAllModifiers(Entity entity)
        {
            if (entity == null) return;
            entity.RemoveAllModifiersBySource(this);
        }

        public void Register(Entity entity)
        {
            if (entity == null) return;
            if (RegisteredEntities.Contains(entity)) return;
            RegisteredEntities.Add(entity);
            AddModifiers(entity);
        }

        public void Unregister(Entity entity)
        {
            if (entity == null) return;
            RegisteredEntities.Remove(entity);
            RemoveAllModifiers(entity);
        }

        #endregion
    }
}