using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces
{
    public interface IRotationalController
    {


        #region Properties

        #endregion

        #region Methods
        bool AimTowardsTarget();
        bool AimTowardsPosition(Vector3 position);
        void RotateTowardsPosition(Vector3 position);
        #endregion
    }
}
