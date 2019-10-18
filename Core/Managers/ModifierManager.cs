using AdventuresUnknownSDK.Core.Objects.Inventories;
using AdventuresUnknownSDK.Core.Objects.Mods;
using AdventuresUnknownSDK.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventuresUnknownSDK.Core.Managers
{
    public abstract class ModifierManager : SingletonBehaviour<ModifierManager>
    {


        #region Properties

        #endregion

        #region Methods
        public static Mod[] GetModifiersForDomain(int domain)
        {
            return Instance.GetModifiersForDomainImpl(domain);
        }
        public static Mod[] GetModifiersForDomainAndTag(int domain,params string[] tags)
        {
            return Instance.GetModifiersForDomainAndTagImpl(domain,tags);
        }
        
        public static void ModifyItemStack(ItemStack itemStack)
        {
            Instance.ModifyItemStackImpl(itemStack);
        }

        protected abstract void ModifyItemStackImpl(ItemStack itemStack);

        protected abstract Mod[] GetModifiersForDomainImpl(int domain);
        protected abstract Mod[] GetModifiersForDomainAndTagImpl(int domain,params string[] tags);
        #endregion
    }
}
