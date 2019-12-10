using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AdventuresUnknownSDK.Core.UI.Tooltip
{
    [RequireComponent(typeof(RectTransform))]
    public class UITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Tooltip m_Tooltip = null;
        [SerializeField] private bool m_UpdateEveryFrame = false;
        [SerializeField] private TextAnchor m_TooltipAnchor = TextAnchor.UpperLeft;
        //[SerializeField] private Vector2 m_TooltipPivot = new Vector2(0.5f, 0.5f);
        [SerializeField] private Vector2 m_Offset = new Vector2(0.0f, 0.0f);
        [SerializeField] private bool m_UseMousePosition = false;
        [SerializeField] private TextAnchor m_RectAnchor = TextAnchor.UpperRight;
        //[SerializeField] private Vector2 m_AnchorPivot = new Vector2(0.5f, 0.5f);
        [NonSerialized] private Func<object> m_Callback = null;

        private bool m_Dirty = false;
        private bool m_IsDisplaying = false;
        private bool m_CanRecalculate = false;
        private Tooltip m_TooltipInstance = null;
        private RectTransform m_RectTransform = null;
        private Canvas m_Canvas = null;

        #region Properties
        public virtual Tooltip Tooltip { get => m_Tooltip; set => m_Tooltip = value; }
        public virtual bool UpdateEveryFrame { get => m_UpdateEveryFrame; set => m_UpdateEveryFrame = value; }
        public virtual Func<object> Callback { get => m_Callback; set => m_Callback = value; }
        protected virtual bool Dirty { get => m_Dirty; set => m_Dirty = value; }
        public bool IsDisplaying { get => m_IsDisplaying; protected set => m_IsDisplaying = value; }
        #endregion

        #region Methods
        private void OnEnable()
        {
            m_RectTransform = GetComponent<RectTransform>();
            m_Canvas = GetComponentInParent<Canvas>();
        }
        private void Update()
        {
            if (!m_IsDisplaying) return;
            if (m_UseMousePosition || m_UpdateEveryFrame)
            {
                Recalculate();
            }
            if (!m_UpdateEveryFrame && !m_Dirty) return;
            m_Dirty = false;
            m_TooltipInstance?.Display(Callback?.Invoke());
        }
        private void OnDisable()
        {
            Hide();
        }
        public void UpdateDisplay()
        {
            m_Dirty = true;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Show();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Hide();
        }
        protected void Recalculate()
        {
            if (!m_CanRecalculate) return;
            //Vector3 anchoredPosition = transform.position;
            //Vector2 rectPivot, tooltipPivot;

            //rectPivot = GetPivot(m_RectAnchor);
            //tooltipPivot = GetPivot(m_TooltipAnchor);

            //if (m_UseMousePosition)
            //{
            //    //other camera modes???
            //    anchoredPosition = m_Canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(m_Offset.x, m_Offset.y));
            //    anchoredPosition.z = m_Canvas.transform.position.z;
            //}
            //else
            //{
            //    anchoredPosition.x -= ((m_RectTransform.pivot.x - rectPivot.x) * m_RectTransform.rect.width + m_Offset.x) * m_RectTransform.lossyScale.x;
            //    anchoredPosition.y -= ((m_RectTransform.pivot.y - rectPivot.y) * m_RectTransform.rect.height + m_Offset.y) * m_RectTransform.lossyScale.y;
            //}



            ////Vector3 anchoredPositionWVP = m_Canvas.worldCamera.ViewportToWorldPoint(anchoredPosition);
            //float width = m_TooltipInstance.RootTransform.rect.width * m_TooltipInstance.RootTransform.lossyScale.x;
            //float height = m_TooltipInstance.RootTransform.rect.height * m_TooltipInstance.RootTransform.lossyScale.y;

            //Vector2 scaledOffset = m_Offset * m_TooltipInstance.RootTransform.lossyScale;

            //float val;

            //Vector3 lowerLeft = m_Canvas.worldCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
            //Vector3 upperRight = m_Canvas.worldCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f));

            //m_TooltipInstance.Anchor(m_TooltipAnchor);


            ////check for right edge of screen
            //val = (anchoredPosition.x + (width * (1 - tooltipPivot.x)));
            //if (val > upperRight.x)
            //{
            //    Vector3 shifter = new Vector3(val - upperRight.x, 0f, 0f);
            //    Vector3 newWorldPos = new Vector3(anchoredPosition.x - shifter.x, anchoredPosition.y, 0f);
            //    anchoredPosition.x = newWorldPos.x;
            //}
            ////check for left edge of screen
            //val = (anchoredPosition.x - (width * tooltipPivot.x));
            //if (val < lowerLeft.x)
            //{
            //    Vector3 shifter = new Vector3(lowerLeft.x - val, 0f, 0f);
            //    Vector3 newWorldPos = new Vector3(anchoredPosition.x + shifter.x, anchoredPosition.y, 0f);
            //    anchoredPosition.x = newWorldPos.x;
            //}
            ////check for upper edge of the screen
            //val = (anchoredPosition.y + (height * (1 - tooltipPivot.y)));
            //if (val > upperRight.y)
            //{
            //    Vector3 shifter = new Vector3(0f, val - upperRight.y, 0f);
            //    Vector3 newWorldPos = new Vector3(anchoredPosition.x, anchoredPosition.y - shifter.y, 0f);
            //    anchoredPosition.y = newWorldPos.y;
            //    //tooltipPivot.y = -m_TooltipPivot.y+1;
            //}
            ////check for upper edge of the screen
            //val = (anchoredPosition.y - (height * tooltipPivot.y));
            //if (val < lowerLeft.y)
            //{
            //    Vector3 shifter = new Vector3(0f, lowerLeft.y - val, 0f);
            //    Vector3 newWorldPos = new Vector3(anchoredPosition.x, anchoredPosition.y + shifter.y, 0f);
            //    anchoredPosition.y = newWorldPos.y;
            //    //tooltipPivot.y = -m_TooltipPivot.y+1;
            //}

            //if (m_TooltipInstance)
            //{
            //    m_TooltipInstance.RootTransform.position = anchoredPosition;
            //    m_TooltipInstance.RootTransform.pivot = tooltipPivot;
            //}

            Vector3 anchoredPosition = transform.position;
            Vector2 rectPivot, tooltipPivot;

            TextAnchor usedTooltipAnchor = m_TooltipAnchor;

            rectPivot = GetPivot(m_RectAnchor);
            tooltipPivot = GetPivot(usedTooltipAnchor);


            if (m_UseMousePosition)
            {
                //other camera modes???
                anchoredPosition = m_Canvas.worldCamera.ScreenToWorldPoint(Input.mousePosition + new Vector3(m_Offset.x, m_Offset.y));
                anchoredPosition.z = m_Canvas.transform.position.z;
            }
            else
            {
                anchoredPosition.x -= ((m_RectTransform.pivot.x - rectPivot.x) * m_RectTransform.rect.width - m_Offset.x) * m_RectTransform.lossyScale.x;
                anchoredPosition.y -= ((m_RectTransform.pivot.y - rectPivot.y) * m_RectTransform.rect.height - m_Offset.y) * m_RectTransform.lossyScale.y;
            }



            //Vector3 anchoredPositionWVP = m_Canvas.worldCamera.ViewportToWorldPoint(anchoredPosition);
            float width = m_TooltipInstance.VisibleTransform.rect.width * m_TooltipInstance.VisibleTransform.lossyScale.x;
            float height = m_TooltipInstance.VisibleTransform.rect.height * m_TooltipInstance.VisibleTransform.lossyScale.y;

            Vector2 scaledOffset = m_Offset * m_TooltipInstance.RootTransform.lossyScale;

            float val;

            Vector3 lowerLeft = m_Canvas.worldCamera.ViewportToWorldPoint(new Vector3(0.0f, 0.0f, 0.0f));
            Vector3 upperRight = m_Canvas.worldCamera.ViewportToWorldPoint(new Vector3(1.0f, 1.0f, 0.0f));

            if (m_UseMousePosition)
            {
                //check for right edge of screen
                val = (anchoredPosition.x + (width * (1 - tooltipPivot.x)));
                if (val > upperRight.x)
                {
                    Vector3 shifter = new Vector3(val - upperRight.x, 0f, 0f);
                    Vector3 newWorldPos = new Vector3(anchoredPosition.x - shifter.x, anchoredPosition.y, 0f);
                    anchoredPosition.x = newWorldPos.x;
                }
                //check for left edge of screen
                val = (anchoredPosition.x - (width * tooltipPivot.x));
                if (val < lowerLeft.x)
                {
                    Vector3 shifter = new Vector3(lowerLeft.x - val, 0f, 0f);
                    Vector3 newWorldPos = new Vector3(anchoredPosition.x + shifter.x, anchoredPosition.y, 0f);
                    anchoredPosition.x = newWorldPos.x;
                }
                //check for upper edge of the screen
                val = (anchoredPosition.y + (height * (1 - tooltipPivot.y)));
                if (val > upperRight.y)
                {
                    Vector3 shifter = new Vector3(0f, val - upperRight.y, 0f);
                    Vector3 newWorldPos = new Vector3(anchoredPosition.x, anchoredPosition.y - shifter.y, 0f);
                    anchoredPosition.y = newWorldPos.y;
                    //tooltipPivot.y = -m_TooltipPivot.y+1;
                }
                //check for upper edge of the screen
                val = (anchoredPosition.y - (height * tooltipPivot.y));
                if (val < lowerLeft.y)
                {
                    Vector3 shifter = new Vector3(0f, lowerLeft.y - val, 0f);
                    Vector3 newWorldPos = new Vector3(anchoredPosition.x, anchoredPosition.y + shifter.y, 0f);
                    anchoredPosition.y = newWorldPos.y;
                    //tooltipPivot.y = -m_TooltipPivot.y+1;
                }
            }
            else
            {
                //right
                val = (anchoredPosition.x + (width * (1 - tooltipPivot.x)));
                if (val > upperRight.x)
                {
                    usedTooltipAnchor = (TextAnchor)(((int)m_TooltipAnchor / 3) * 3 + (2 - (int)m_TooltipAnchor % 3));
                    tooltipPivot = GetPivot(usedTooltipAnchor);
                    rectPivot = GetPivot((TextAnchor)(((int)m_RectAnchor / 3) * 3 + (2 - (int)m_RectAnchor % 3)));
                    anchoredPosition = transform.position;
                    anchoredPosition.x -= ((m_RectTransform.pivot.x - rectPivot.x) * m_RectTransform.rect.width + m_Offset.x) * m_RectTransform.lossyScale.x;
                    anchoredPosition.y -= ((m_RectTransform.pivot.y - rectPivot.y) * m_RectTransform.rect.height + m_Offset.y) * m_RectTransform.lossyScale.y;
                }
                //left
                val = (anchoredPosition.x - (width * tooltipPivot.x));
                if (val < lowerLeft.x)
                {
                    usedTooltipAnchor = (TextAnchor)(((int)m_TooltipAnchor / 3) * 3 + (2 - (int)m_TooltipAnchor % 3));
                    tooltipPivot = GetPivot(usedTooltipAnchor);
                    rectPivot = GetPivot((TextAnchor)(((int)m_RectAnchor / 3) * 3 + (2 - (int)m_RectAnchor % 3)));
                    anchoredPosition = transform.position;
                    anchoredPosition.x -= ((m_RectTransform.pivot.x - rectPivot.x) * m_RectTransform.rect.width + m_Offset.x) * m_RectTransform.lossyScale.x;
                    anchoredPosition.y -= ((m_RectTransform.pivot.y - rectPivot.y) * m_RectTransform.rect.height + m_Offset.y) * m_RectTransform.lossyScale.y;
                }//check for upper edge of the screen
                val = (anchoredPosition.y + (height * (1 - tooltipPivot.y)));
                if (val > upperRight.y)
                {
                    Vector3 shifter = new Vector3(0f, val - upperRight.y, 0f);
                    Vector3 newWorldPos = new Vector3(anchoredPosition.x, anchoredPosition.y - shifter.y, 0f);
                    anchoredPosition.y = newWorldPos.y;
                    //tooltipPivot.y = -m_TooltipPivot.y+1;
                }
                //check for upper edge of the screen
                val = (anchoredPosition.y - (height * tooltipPivot.y));
                if (val < lowerLeft.y)
                {
                    Vector3 shifter = new Vector3(0f, lowerLeft.y - val, 0f);
                    Vector3 newWorldPos = new Vector3(anchoredPosition.x, anchoredPosition.y + shifter.y, 0f);
                    anchoredPosition.y = newWorldPos.y;
                    //tooltipPivot.y = -m_TooltipPivot.y+1;
                }
            }

            m_TooltipInstance.Anchor(usedTooltipAnchor);

            if (m_TooltipInstance)
            {
                m_TooltipInstance.RootTransform.position = anchoredPosition;
                m_TooltipInstance.RootTransform.pivot = tooltipPivot;
            }
        }

        private Vector2 GetPivot(TextAnchor anchor)
        {
            float x = ((int)anchor % 3) / 2.0f;
            float y = (2 - ((int)anchor / 3)) / 2.0f;

            return new Vector2(x, y);
        }

        protected virtual void Show()
        {
            if (m_IsDisplaying) return;
            m_IsDisplaying = true;
            m_Dirty = true;

            Canvas parentCanvas = transform.GetComponentInParent<Canvas>();

            if (m_Tooltip == null) return;

            m_TooltipInstance = Instantiate(m_Tooltip, parentCanvas.transform);
            m_TooltipInstance.transform.position = new Vector3(10000, 10000);
            if (!m_TooltipInstance.RootTransform)
            {
                Hide();
                return;
            }
            StartCoroutine(WaitAndRecalculate());
        }

        private IEnumerator WaitAndRecalculate()
        {
            yield return new WaitForEndOfFrame();
            m_CanRecalculate = true;

            Recalculate();

            yield break;
        }

        protected virtual void Hide()
        {
            if (!m_IsDisplaying) return;
            m_CanRecalculate = false;
            m_IsDisplaying = false;
            Destroy(m_TooltipInstance?.gameObject);
        }
        #endregion
    }
}
