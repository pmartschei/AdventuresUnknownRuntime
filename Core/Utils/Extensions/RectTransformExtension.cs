using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace AdventuresUnknownSDK.Core.Utils.Extensions
{
    public static class RectTransformExtension
    {
        public static IEnumerator PositionNextTo(this RectTransform origin, RectTransform target)
        {
            yield return new WaitForEndOfFrame();
            if (target)
            {
                Vector2 originSize = origin.sizeDelta * origin.lossyScale;
                Vector2 targetSize = target.sizeDelta * target.lossyScale;
                Vector2 originPivot = origin.pivot;
                Vector2 targetPivot = target.pivot;

                Vector2 targetPosition = target.position;

                Vector3 originOffset = new Vector3();
                originOffset.x = originPivot.x * originSize.x;
                originOffset.y = -(1.0f-originPivot.y) * originSize.y;

                Vector3 targetOffset = new Vector3();
                targetOffset.x = (1.0f-targetPivot.x) * targetSize.x;
                targetOffset.y = (1.0f-targetPivot.y) * targetSize.y;

                Vector3 result = target.position + originOffset + targetOffset;

                Vector3 rightOffset = new Vector3();
                rightOffset.x = originSize.x * (1.0f - originPivot.x);
                rightOffset.y = -originSize.y * originPivot.y;

                Vector3 viewPortCheck = result + rightOffset;
                Vector3 viewPort = Camera.main.ScreenToViewportPoint(viewPortCheck);

                if (viewPort.x > 1.0f)
                {
                    result.x += -originSize.x - targetSize.x;
                }
                if (viewPort.y < 0.0f)
                {
                    result.y += originSize.y - targetSize.y;
                }

                origin.position = result;
            }
            yield return null;
        }
    }
}
