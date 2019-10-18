using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace AdventuresUnknownSDK.Core.Utils.Extensions
{
    public static class RectTransformExtension
    {
        public enum PositionAlignment
        {
            Top,
            Center,
            Bottom,
        }
        public static IEnumerator PositionNextTo(this RectTransform origin, RectTransform target, PositionAlignment alignment = PositionAlignment.Top)
        {
            yield return new WaitForEndOfFrame();
            if (target)
            {
                Vector2 originSize = origin.sizeDelta * origin.lossyScale;
                Vector2 targetSize = target.sizeDelta * target.lossyScale;

                Vector2 originPivot = origin.pivot;
                Vector2 targetPivot = target.pivot;

                Vector3 targetPosition = target.position;

                Vector3 originOffset = new Vector3();

                originOffset.x = originPivot.x * originSize.x;
                if (alignment == PositionAlignment.Top)
                {
                    originOffset.y = -(1.0f - originPivot.y) * originSize.y;
                }
                else if (alignment == PositionAlignment.Bottom)
                {
                    originOffset.y = (1.0f - originPivot.y) * originSize.y;
                }

                Vector3 targetOffset = new Vector3();
                targetOffset.x = (1.0f - targetPivot.x) * targetSize.x;
                if (alignment == PositionAlignment.Top)
                {
                    targetOffset.y = (1.0f - targetPivot.y) * targetSize.y;
                }
                else if (alignment == PositionAlignment.Bottom)
                {
                    targetOffset.y = -(1.0f - targetPivot.y) * targetSize.y;
                }
                Vector3 result = targetPosition + originOffset + targetOffset;

                Vector3 rightOffset = new Vector3();
                rightOffset.x = originSize.x * (1.0f - originPivot.x);
                rightOffset.y = -originSize.y * originPivot.y;

                Vector3 viewPortCheck = result + rightOffset;
                Canvas canvas = origin.root.GetComponent<Canvas>();
                Camera camera = Camera.main;
                Vector3 viewPort;
                if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
                {
                    camera = canvas.worldCamera;
                    viewPort = camera.WorldToViewportPoint(viewPortCheck);
                }
                else
                {
                    viewPort = camera.ScreenToViewportPoint(viewPortCheck);
                }


                if (viewPort.x > 1.0f)
                {
                    result.x += -originSize.x - targetSize.x;
                    LayoutGroup layoutGroup = origin.GetComponent<LayoutGroup>();
                    if (layoutGroup)
                    {
                        layoutGroup.childAlignment = TextAnchor.UpperRight;
                    }
                }
                else
                {
                    LayoutGroup layoutGroup = origin.GetComponent<LayoutGroup>();
                    if (layoutGroup)
                    {
                        layoutGroup.childAlignment = TextAnchor.UpperLeft;
                    }
                }
                if (viewPort.y < 0.0f)
                {
                    result.y += originSize.y - targetSize.y;
                    //TODO
                    //result.y += -viewPort.y * originSize.y*2;
                    //result.y += originSize.y - targetSize.y;
                }

                origin.position = result;
            }
            yield break;
        }
        public static IEnumerator PositionNextTo(this RectTransform origin, Vector3 target, PositionAlignment alignment = PositionAlignment.Top,Vector2 offset = default)
        {
            yield return new WaitForEndOfFrame();
            Vector2 originSize = origin.sizeDelta * origin.lossyScale;

            Vector2 offsetSize = offset * origin.lossyScale;

            Vector2 originPivot = origin.pivot;

            Vector3 targetPosition = target;

            Vector3 originOffset = new Vector3();

            originOffset.x = originPivot.x * originSize.x + offsetSize.x;
            if (alignment == PositionAlignment.Top)
            {
                originOffset.y = -(1.0f - originPivot.y) * originSize.y - offsetSize.y;
            }
            else if (alignment == PositionAlignment.Bottom)
            {
                originOffset.y = (1.0f - originPivot.y) * originSize.y + +offsetSize.y;
            }

            Vector3 result = targetPosition + originOffset;

            Vector3 rightOffset = new Vector3();
            rightOffset.x = originSize.x * (1.0f - originPivot.x);
            rightOffset.y = -originSize.y * originPivot.y;

            Vector3 viewPortCheck = result + rightOffset;
            Canvas canvas = origin.root.GetComponent<Canvas>();
            Camera camera = Camera.main;
            Vector3 viewPort;
            if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
            {
                camera = canvas.worldCamera;
                viewPort = camera.WorldToViewportPoint(viewPortCheck);
            }
            else
            {
                viewPort = camera.ScreenToViewportPoint(viewPortCheck);
            }


            if (viewPort.x > 1.0f)
            {
                result.x += -originSize.x - offsetSize.x;
                LayoutGroup layoutGroup = origin.GetComponent<LayoutGroup>();
                if (layoutGroup)
                {
                    layoutGroup.childAlignment = TextAnchor.UpperRight;
                }
            }
            else
            {
                LayoutGroup layoutGroup = origin.GetComponent<LayoutGroup>();
                if (layoutGroup)
                {
                    layoutGroup.childAlignment = TextAnchor.UpperLeft;
                }
            }
            if (viewPort.y < 0.0f)
            {
                result.y += originSize.y;
                //TODO
                //result.y += -viewPort.y * originSize.y*2;
                //result.y += originSize.y - targetSize.y;
            }

            origin.position = result;
            yield break;
        }
    }
}
