using AdventuresUnknownSDK.Core.Attributes;
using AdventuresUnknownSDK.Core.Entities;
using AdventuresUnknownSDK.Core.Entities.Controllers;
using AdventuresUnknownSDK.Core.Utils.Identifiers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class UIHealthBar : MonoBehaviour
{
    [SerializeField] private Slider m_Slider = null;
    [SerializeField] private ModTypeIdentifier m_ModType = null;
    [SerializeField] private CanvasGroup m_CanvasGroup = null;

    private RectTransform m_RectTransform = null;
    private EntityController m_EntityController = null;
    private Vector3 m_Offset = Vector3.zero;
    private Vector3 m_Scale = Vector3.zero;
    private Vector3 m_InvertedScale = Vector3.zero;
    #region Properties
    public Vector3 Offset { get => m_Offset; set => m_Offset = value; }
    public EntityController EntityController { get => m_EntityController; set => m_EntityController = value; }
    public Vector3 InvertedScale
    {
        get
        {
            if (m_Scale != this.transform.root.transform.lossyScale)
            {
                m_Scale = this.transform.root.transform.lossyScale;
                m_InvertedScale = new Vector3(1.0f / m_Scale.x, 1.0f / m_Scale.y, 1.0f / m_Scale.z);
            }
            return m_InvertedScale;
        }
    }
    #endregion

    #region Methods
    private void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
    }
    void LateUpdate()
    {
        if (!EntityController)
        {
            Destroy(this.gameObject);
            return;
        }
        if (m_CanvasGroup)
            m_CanvasGroup.alpha = 1.0f;
        Vector3 position = EntityController.Head.position;
        position += m_Offset;
        Vector3 viewPortPosition = Camera.main.WorldToViewportPoint(position);
        viewPortPosition = new Vector3(
            (viewPortPosition.x-0.5f) * this.transform.root.GetComponent<RectTransform>().sizeDelta.x,
            (viewPortPosition.y-0.5f) * this.transform.root.GetComponent<RectTransform>().sizeDelta.y);
        this.transform.localPosition = viewPortPosition;
        if (m_Slider)
        {
            Stat stat = EntityController.SpaceShip.Entity.GetStat(m_ModType.Identifier);
            m_Slider.maxValue = stat.Calculated;
            m_Slider.minValue = 0.0f;
            m_Slider.value = stat.Current;
        }
    }

    public void SetWidthRelativeInGame(float width)
    {
        m_RectTransform.sizeDelta = new Vector2(width * InvertedScale.x,m_RectTransform.sizeDelta.y);
    }
    #endregion
}
