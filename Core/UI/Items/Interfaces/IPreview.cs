using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI.Items.Interfaces
{
    public abstract class IPreview : MonoBehaviour
    {
        public abstract void Hide();
        public abstract void Show(bool valid);
    }
}
