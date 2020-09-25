﻿using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class ShapeTweens : MonoBehaviour
{
    public enum FromMode
    {
        None,
        Dynamic,
        FromValue
    }

    public RectTransform pivot;
    public float duration = 2;
    public Ease ease = Ease.Linear;
    public FromMode fromMode = FromMode.None;
    public bool isRelative;
    public int loops;
    public LoopType loopType = LoopType.Yoyo;
    public Circle[] circleTweens;

    void Start()
    {
        for (int i = 0; i < circleTweens.Length; i++) circleTweens[i].Init(this, pivot);
    }

    // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████
    // ███ INTERNAL CLASSES ████████████████████████████████████████████████████████████████████████████████████████████████
    // █████████████████████████████████████████████████████████████████████████████████████████████████████████████████████

    [Serializable]
    public abstract class Shape2D
    {
        public RectTransform target;
        public bool useRelativeCenter;
        public Vector2 relativeCenter = Vector2.zero; // If Vector2.zero is ignored
        public bool snapping;

        public void Init(ShapeTweens data, RectTransform pivot)
        {
            if (target == null) return;
            DOVirtual.DelayedCall(1, () => {
                Execute(data, pivot);
            }, false);
        }

        protected abstract void Execute(ShapeTweens data, RectTransform pivot);
    }

    [Serializable]
    public class Circle : Shape2D
    {
        public float degrees = 360;
        public float fromDegrees;

        protected override void Execute(ShapeTweens data, RectTransform pivot)
        {
            var t = target.DOShapeCircle(useRelativeCenter ? relativeCenter : pivot.anchoredPosition, degrees, data.duration, useRelativeCenter, snapping);
            if (data.fromMode != FromMode.None) {
                if (data.fromMode == FromMode.Dynamic) t.From(data.isRelative);
                else t.From(fromDegrees, true, data.isRelative);
            } else t.SetRelative(data.isRelative);
            t.SetEase(data.ease)
                .SetLoops(data.loops, data.loopType);
        }
    }
}