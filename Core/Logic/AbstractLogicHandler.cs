using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace AdventuresUnknownSDK.Core.Logic
{
    public abstract class AbstractLogicHandler
    {
        //could use priority to sort logichandlers but probably not needed when we use afterinit etc
        //[SerializeField] private int m_Priority = 0;

        #region Properties

        #endregion

        #region Methods
        public abstract void Init();
        //public virtual void AfterInit() { }
        //public virtual void BeforeStartUp() { }
        //public virtual void StartUp() { }
        //public virtual void AfterStartUp() { }
        //public virtual void OnSceneSwitch() { }
        //public virtual void OnUpdate() { }
        //public virtual void OnDeath() { }
        #endregion
    }
}
