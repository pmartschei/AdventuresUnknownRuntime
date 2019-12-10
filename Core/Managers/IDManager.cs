using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public static class IDManager
    {

        private static ulong m_ID = 0;
        public static ulong GetUniqueID()
        {
            m_ID++;
            if (m_ID == 0)
            {
                m_ID++;
            }
            return m_ID;
        }

    }
}
