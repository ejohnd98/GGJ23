using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EasingType{
        INOUT_CUBIC,
        INOUT_QUAD,
        IN_CUBIC,
        LINEAR
    }

public class Easings
{

    static public float EaseInOutCubic(float x) {
        return Mathf.Clamp01(x < 0.5 ? 4 * x * x * x : 1 - Mathf.Pow(-2 * x + 2, 3) / 2);
    }

    static public float EaseInOutQuad(float x) {
        return Mathf.Clamp01(x < 0.5 ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2);
    }

    static public float EaseInCubic(float x){
        return Mathf.Clamp01(x * x * x);
    }

    static public float EaseIn(float x, EasingType type){
        switch(type){
            case EasingType.INOUT_CUBIC:
                return EaseInOutCubic(x);
            case EasingType.INOUT_QUAD:
                return EaseInOutQuad(x);
            case EasingType.IN_CUBIC:
                return EaseInCubic(x);
            case EasingType.LINEAR:
            default:
                return x;
        }
    }
}
