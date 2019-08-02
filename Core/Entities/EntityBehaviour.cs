using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities
{
    public class EntityBehaviour : MonoBehaviour
    {

        [SerializeField] private Entity m_Entity = new Entity();


        #region Properties
        public Entity Entity { get => m_Entity; set => m_Entity = value; }
        #endregion

        #region Methods
        private void Start()
        {
            Entity.GameObject = this.gameObject;
            Entity.Start();
        }
        private void Update()
        {
            Entity.Tick(Time.deltaTime);
        }
        #endregion
    }
}
