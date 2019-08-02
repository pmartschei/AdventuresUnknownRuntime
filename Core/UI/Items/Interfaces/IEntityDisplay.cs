using AdventuresUnknownSDK.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Items.Interfaces
{
    public abstract class IEntityDisplay : MonoBehaviour
    {


        #region Properties

        #endregion

        #region Methods
        public abstract bool Display(Entity entity,string formatter, string[] modTypes);
        #endregion
    }
}
