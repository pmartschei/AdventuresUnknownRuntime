using AdventuresUnknownSDK.Core.Attributes;
using AdventuresUnknownSDK.Core.Managers;
using AdventuresUnknownSDK.Core.Objects.Localization;
using System;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Objects.Mods
{
    [CreateAssetMenu(menuName = "AdventuresUnknown/Core/Mods/ModTypeFormatter", fileName = "ModTypeFormatter.asset")]
    public class ModTypeFormatter : CoreObject
    {
        [Serializable]
        public struct InternalFormat
        {
            [SerializeField] private LocalizationString m_Pre;
            [SerializeField] private LocalizationString m_Main;
            [SerializeField] private LocalizationString m_Post;
            public InternalFormat(string pre, string main, string post)
            {
                m_Pre = new LocalizationString();
                m_Pre.LocalizedIdentifier = pre;
                m_Main = new LocalizationString();
                m_Main.LocalizedIdentifier = main;
                m_Post = new LocalizationString();
                m_Post.LocalizedIdentifier = post;
            }

            public LocalizationString Pre { get => m_Pre; set => m_Pre = value; }
            public LocalizationString Main { get => m_Main; set => m_Main = value; }
            public LocalizationString Post { get => m_Post; set => m_Post = value; }
        }

        [LowerCaseOnly]
        [SerializeField] private string m_Type = "empty_identifier";
        [SerializeField] private LocalizationString m_Description = new LocalizationString("core.modtypeformatter.default.description");
        [SerializeField] private InternalFormat m_AdditionalFormat = new InternalFormat(
            "core.modtypeformatters.default.flatpre", 
            "core.modtypeformatters.default.flatmain", 
            "core.modtypeformatters.default.flatpost");
        [SerializeField] private InternalFormat m_IncreasedFormat = new InternalFormat(
            "core.modtypeformatters.default.increasedpre",
            "core.modtypeformatters.default.increasedmain",
            "core.modtypeformatters.default.increasedpost");
        [SerializeField] private InternalFormat m_MoreFormat = new InternalFormat(
            "core.modtypeformatters.default.morepre",
            "core.modtypeformatters.default.moremain",
            "core.modtypeformatters.default.morepost");


        #region Properties
        public static ModTypeFormatter DefaultFormatter => GameSettingsManager.DefaultModTypeFormatter;
        public string Type { get => m_Type; private set => m_Type = value; }
        public string Description { get => m_Description.LocalizedString; }
        #endregion

        #region Methods
        public virtual string ToText(float value,ModType modType, CalculationType calculationType,string htmlColor)
        {
            InternalFormat format = m_AdditionalFormat;
            switch (calculationType)
            {
                case CalculationType.Increased:
                    format = m_IncreasedFormat;
                    break;
                case CalculationType.More:
                    format = m_MoreFormat;
                    break;
            }
            StringBuilder sb = new StringBuilder();
            string[] preSplits = format.Pre.LocalizedString.Split(';');
            string[] postSplits = format.Post.LocalizedString.Split(';');
            string preString = preSplits[0];
            string postString = postSplits[0];
            if (value < 0)
            {
                if (preSplits.Length >= 2)
                {
                    preString = preSplits[1];
                }
                if (postSplits.Length >= 2)
                {
                    postString = postSplits[1];
                }
            }
            sb.Append(preString+(!preString.Equals(string.Empty)?" ":""));
            sb.Append("<b>");
            sb.AppendFormat("{0:" + format.Main + (format.Main.LocalizedString.Contains(";") ? "" : ";" + format.Main) + "}",value);
            sb.Append(format.Main.Equals(string.Empty) ? "" : " ");
            sb.Append("</b>");
            sb.Append(postString+(!postString.Equals(string.Empty) ? " " : ""));
            sb.Append("<color=#" + htmlColor + ">" + Description + "</color>");
            return sb.ToString();
        }
        public string ToText(float value, ModType modType, CalculationType calculationType)
        {
            return ToText(value, modType, calculationType, "00000000");
        }
        public virtual string ToFormat(float value, CalculationType calculationType)
        {
            InternalFormat format = m_AdditionalFormat;
            switch (calculationType)
            {
                case CalculationType.Increased:
                    format = m_IncreasedFormat;
                    break;
                case CalculationType.More:
                    format = m_MoreFormat;
                    break;
            }
            return string.Format("{0:" + format.Main + (format.Main.LocalizedString.Contains(";") ? "" : ";" + format.Main) + "}", value);
        }
        public override void ForceUpdate()
        {
            base.ForceUpdate();
            m_Description.ForceUpdate();
            m_AdditionalFormat.Main.ForceUpdate();
            m_AdditionalFormat.Pre.ForceUpdate();
            m_AdditionalFormat.Post.ForceUpdate();
            m_IncreasedFormat.Main.ForceUpdate();
            m_IncreasedFormat.Pre.ForceUpdate();
            m_IncreasedFormat.Post.ForceUpdate();
            m_MoreFormat.Main.ForceUpdate();
            m_MoreFormat.Pre.ForceUpdate();
            m_MoreFormat.Post.ForceUpdate();
        }
        #endregion
    }
}