using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Entities.Controllers.Interfaces
{
    public interface ITranslationalController
    {


        #region Properties

        #endregion

        #region Methods
        bool MoveTowardsTarget();
        bool MoveTowardsPosition(Vector3 position);
        Vector3[] GetNavPoints();
        void ResumeMove();
        void StopMove();
        #endregion
    }
}
