using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Objects.Mods.Actions.ActionObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods.Actions
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/Actions/ModActionCollection", fileName = "ModActionCollection.asset")]
    public class ModActionCollection : ScriptableObject
    {
        [SerializeField] private List<BaseAction> m_BaseActions = new List<BaseAction>();

        #region Properties
        public BaseAction[] BaseActions { get => m_BaseActions.ToArray(); }
        #endregion

        #region Methods
        public void Initialize(ModType modType)
        {
            foreach (BaseAction baseAction in m_BaseActions)
            {
                if (!baseAction) continue;
                baseAction.Initialize(modType);
            }
        }
        #endregion
    }
}
