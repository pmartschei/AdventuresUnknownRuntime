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

        public static ActionType Calculation                { get => Instance.CalculationImpl; set => Instance.CalculationImpl = value; }
        public static ActionType Tick                       { get => Instance.TickImpl; set => Instance.TickImpl = value; }
        public static ActionType Death                      { get => Instance.DeathImpl; set => Instance.DeathImpl = value; }
        public static ActionType Spawn                      { get => Instance.SpawnImpl; set => Instance.SpawnImpl = value; }
        public static ActionType Block                      { get => Instance.BlockImpl; set => Instance.BlockImpl = value; }
        public static ActionType HitGeneration              { get => Instance.HitGenerationImpl; set => Instance.HitGenerationImpl = value; }
        public static ActionType HitCalculation             { get => Instance.HitCalculationImpl; set => Instance.HitCalculationImpl = value; }
        public static ActionType HitApply                   { get => Instance.HitApplyImpl; set => Instance.HitApplyImpl = value; }
        public static ActionType AttackCooldownGeneration   { get => Instance.AttackCooldownGenerationImpl; set => Instance.AttackCooldownGenerationImpl = value; }
        public static ActionType AttackCooldownApply        { get => Instance.AttackCooldownApplyImpl; set => Instance.AttackCooldownApplyImpl = value; }
        public static ActionType AttackGeneration           { get => Instance.AttackGenerationImpl; set => Instance.AttackGenerationImpl = value; }

        protected abstract ActionType CalculationImpl { get; set; }
        protected abstract ActionType TickImpl { get; set; }
        protected abstract ActionType DeathImpl { get; set; }
        protected abstract ActionType SpawnImpl { get; set; }
        protected abstract ActionType BlockImpl { get; set; }
        protected abstract ActionType HitGenerationImpl { get; set; }
        protected abstract ActionType HitCalculationImpl { get; set; }
        protected abstract ActionType HitApplyImpl { get; set; }
        protected abstract ActionType AttackCooldownGenerationImpl { get; set; }
        protected abstract ActionType AttackCooldownApplyImpl { get; set; }
        protected abstract ActionType AttackGenerationImpl { get; set; }
        #endregion

        #region Methods

        #endregion
    }
}
