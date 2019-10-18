using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Currencies;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.UI
{
    public class UIWalletDisplay : MonoBehaviour
    {
        [SerializeField] private UICurrencyText[] m_WalletDisplays = null;

        #region Properties

        #endregion

        #region Methods
        private void Start()
        {
            OnWalletDisplayChange();
        }
        private void OnEnable()
        {
            PlayerManager.OnWalletDisplayChange.AddListener(OnWalletDisplayChange);
        }

        private void OnDisable()
        {
            PlayerManager.OnWalletDisplayChange.RemoveListener(OnWalletDisplayChange);
        }

        private void OnWalletDisplayChange()
        {
            Currency[] currencies = PlayerManager.WalletDisplay;
            for(int i = 0; i < currencies.Length && i < m_WalletDisplays.Length; i++)
            {
                m_WalletDisplays[i].gameObject.SetActive(true);
                m_WalletDisplays[i].SetCurrency(currencies[i]);
                m_WalletDisplays[i].SetText(PlayerManager.PlayerWallet.GetValue(currencies[i].Identifier).ToString());
            }
            for(int i = currencies.Length;i < m_WalletDisplays.Length; i++)
            {
                if (m_WalletDisplays[i] == null) continue;
                m_WalletDisplays[i].gameObject.SetActive(false);
            }
        }
        #endregion
    }
}
