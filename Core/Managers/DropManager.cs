using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.DropTables;
using AdventuresUnknownSDK.Core.Objects.Enemies;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class DropManager : SingletonBehaviour<DropManager>
    {


        #region Properties

        #endregion

        #region Methods
        public static void SetDropRate(Enemy enemy,DropTable dropTable,float rate, int count)
        {
            Instance.SetDropRateImpl(enemy, dropTable, rate, count);
        }
        public static void RemoveDropRate(Enemy enemy,DropTable dropTable)
        {
            Instance.RemoveDropRateImpl(enemy, dropTable);
        }
        public static void GenerateDrop(ItemStack itemStack, Vector3 position)
        {
            Instance.GenerateDropImpl(itemStack, position);
        }
        public static DropRate[] GetDropRates(Enemy enemy)
        {
            return Instance.GetDropRatesImpl(enemy);
        }
        protected abstract void GenerateDropImpl(ItemStack itemStack, Vector3 position);
        protected abstract void SetDropRateImpl(Enemy enemy, DropTable dropTable, float rate, int count);
        protected abstract void RemoveDropRateImpl(Enemy enemy, DropTable dropTable);
        protected abstract DropRate[] GetDropRatesImpl(Enemy enemy);
        #endregion
    }
}
