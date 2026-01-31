using System.Collections;
using UnityEngine;

public class TextAnimation : MonoBehaviour
{
    public RectTransform target;
    public float duration = 0.3f;
    public AnimationCurve scaleCurve;

    float t;

    void Reset()
    {
        // example default curve: 1 -> 0.5 -> 1
        scaleCurve = new AnimationCurve(
            new Keyframe(0f,   1f),
            new Keyframe(0.5f, 0.5f),
            new Keyframe(1f,   1f)
        );
    }

    void Update()
    {
        if (target == null) return;

        t += Time.unscaledDeltaTime;
        if (t > duration) t -= duration;

        float normalized = t / duration;           // 0..1
        float scaleMul  = scaleCurve.Evaluate(normalized); // from curve

        target.localScale = Vector3.one * scaleMul;
    }
}
