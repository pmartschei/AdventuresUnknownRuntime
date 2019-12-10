using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Entities.Weapons;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Items;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Logic.ActiveGemContainers
{
    public abstract class GenericActiveGemContainer : MonoBehaviour
    {
        private EntityBehaviour m_EntityStats;
        [SerializeField] private string m_ContainerName = "";
        [SerializeField] private List<Entity> m_ActiveStats = new List<Entity>();


        #region Properties
        public List<Entity> ActiveStats { get => m_ActiveStats; }
        public EntityBehaviour EntityStats { get => m_EntityStats; protected set => m_EntityStats = value; }
        public string ContainerName { get => m_ContainerName; set => m_ContainerName = value; }
        #endregion

        #region Methods
        protected void UpdateListSizes<T>(List<T> list,int size)
        {
            if (list == null) return;
            int diff = list.Count - size;
            if (diff == 0) return;
            if (diff > 0)
            {
                list.RemoveRange(size, diff);
            }
            else
            {
                for(int i = 0; i < -diff; i++)
                {
                    list.Add(default(T));
                }
            }
        }

        public abstract Entity CalculateEntity(int index);
        public virtual Entity GetEntityWithApply(int index)
        {
            return CalculateEntity(index);
        }
        public virtual string[] CalculateDisplayMods(int index)
        {
            return new string[0];
        }
        public abstract void Spawn(EntityController origin,int index,params Muzzle[] muzzles);
        #endregion
    }
}
