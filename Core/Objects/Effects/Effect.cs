using AdventuresUnknownSDK.Core.Objects.Mods.ModBases;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Effects
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Effects/Effect", fileName = "Effect.asset")]
    public class Effect : CoreObject
    {
        [SerializeField] private BasicModBaseIdentifier m_BasicModBaseIdentifier = null;
        [SerializeField] private Sprite m_Icon = null;
        [SerializeField] private bool m_ShowIcon = true;

        public BasicModBase BasicModBase { get => m_BasicModBaseIdentifier.Object; }
        public Sprite Icon { get => m_Icon; set => m_Icon = value; }
        public bool ShowIcon { get => m_ShowIcon; set => m_ShowIcon = value; }

        public override bool ConsistencyCheck()
        {
            if (!base.ConsistencyCheck()) return false;

            return m_BasicModBaseIdentifier.ConsistencyCheck();
        }

        public void Update()
        {

        }
        public virtual void Update(EffectContext effectContext,float time)
        {
            effectContext.Duration -= time;
        }
        public virtual bool IsGone(EffectContext effectContext)
        {
            return !effectContext.IsInfinityDuration && effectContext.Duration <= 0.0f;
        }
        public virtual void AddEffect(EffectContext effectContext,float duration,float value)
        {
            effectContext.Duration = Mathf.Max(effectContext.Duration, duration);
            effectContext.Value = value;
        }
    }
}
