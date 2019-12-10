using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Tooltip
{
    public class ListTooltip : Tooltip<List<int>>
    {
        public override void Display(List<int> t)
        {
            if (t == null) return;
            Debug.Log(t.Count);
            for (int i = 0; i < t.Count; i++)
            {
                Debug.Log(i+": "+t[i]);
            }
        }
    }
}
