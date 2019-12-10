using AdventuresUnknownSDK.Core.Objects.Mods.Actions;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class ActionTypeManager : SingletonBehaviour<ActionTypeManager>
    {

        #region Properties

        public static ActionType Immediate                { get => Instance.ImmediateImpl; set => Instance.ImmediateImpl = value; }
        public static ActionType Tick                       { get => Instance.TickImpl; set => Instance.TickImpl = value; }
        public static ActionType Death                      { get => Instance.DeathImpl; set => Instance.DeathImpl = value; }
        public static ActionType PostDeath                  { get => Instance.PostDeathImpl; set => Instance.PostDeathImpl = value; }
        public static ActionType Spawn                      { get => Instance.SpawnImpl; set => Instance.SpawnImpl = value; }
        public static ActionType Block                      { get => Instance.BlockImpl; set => Instance.BlockImpl = value; }
        public static ActionType HitGenerationOffensive     { get => Instance.HitGenerationOffensiveImpl; set => Instance.HitGenerationOffensiveImpl = value; }
        public static ActionType HitGenerationDefensive     { get => Instance.HitGenerationDefensiveImpl; set => Instance.HitGenerationDefensiveImpl = value; }
        public static ActionType HitCalculation             { get => Instance.HitCalculationImpl; set => Instance.HitCalculationImpl = value; }
        public static ActionType HitApply                   { get => Instance.HitApplyImpl; set => Instance.HitApplyImpl = value; }
        public static ActionType AttackCooldownGeneration   { get => Instance.AttackCooldownGenerationImpl; set => Instance.AttackCooldownGenerationImpl = value; }
        public static ActionType AttackCooldownApply        { get => Instance.AttackCooldownApplyImpl; set => Instance.AttackCooldownApplyImpl = value; }
        public static ActionType AttackApply                { get => Instance.AttackApplyImpl; set => Instance.AttackApplyImpl = value; }
        public static ActionType AttackGeneration           { get => Instance.AttackGenerationImpl; set => Instance.AttackGenerationImpl = value; }
        public static ActionType AuraApply                  { get => Instance.AuraApplyImpl; set => Instance.AuraApplyImpl = value; }

        protected abstract ActionType ImmediateImpl { get; set; }
        protected abstract ActionType TickImpl { get; set; }
        protected abstract ActionType DeathImpl { get; set; }
        protected abstract ActionType PostDeathImpl { get; set; }
        protected abstract ActionType SpawnImpl { get; set; }
        protected abstract ActionType BlockImpl { get; set; }
        protected abstract ActionType HitGenerationOffensiveImpl { get; set; }
        protected abstract ActionType HitGenerationDefensiveImpl { get; set; }
        protected abstract ActionType HitCalculationImpl { get; set; }
        protected abstract ActionType HitApplyImpl { get; set; }
        protected abstract ActionType AttackCooldownGenerationImpl { get; set; }
        protected abstract ActionType AttackCooldownApplyImpl { get; set; }
        protected abstract ActionType AttackApplyImpl { get; set; }
        protected abstract ActionType AttackGenerationImpl { get; set; }
        protected abstract ActionType AuraApplyImpl { get; set; }
        #endregion

        #region Methods

        #endregion
    }
}
