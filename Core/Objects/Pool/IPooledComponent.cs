using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Objects.Pool
{
    public interface IPooledComponent
    {
        void OnSpawn();
        void OnPreEnable();
        void OnPostEnable();
        void OnPreDisable();
        void OnPostDisable();
    }
}
