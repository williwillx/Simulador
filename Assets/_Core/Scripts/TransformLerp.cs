using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TransformLerp : MonoBehaviour
{
    bool hasLerped = false;
    [SerializeField] float animationTime;

    [Header("Position config")]
    [SerializeField] bool lerpPosition;
    [SerializeField] Vector3 finalPosition;
    Vector3 originalPosition;

    [Header("Euler Angles config")]
    [SerializeField] bool lerpEulerAngles;
    [SerializeField] Vector3 finalEulerAngles;
    Vector3 originalEuler;

    [Header("Scale config")]
    [SerializeField] bool lerpScale;
    [SerializeField] Vector3 finalScale;
    Vector3 originalScale;

    [Header("Delay Config")]
    [SerializeField] float delay;

    private void Start()
    {
        originalPosition = transform.position;
        originalEuler = transform.eulerAngles;
        originalScale = transform.localScale;
    }

    public void ToggleAllLerps()
    {
        if (!hasLerped)
        {
            if (lerpPosition)
                transform.DOMove(finalPosition, animationTime).SetDelay(delay);

            if (lerpEulerAngles)
                transform.DORotate(finalEulerAngles, animationTime).SetDelay(delay);

            if (lerpScale)
                transform.DOScale(finalScale, animationTime).SetDelay(delay);
        }
        else
        {
            if (lerpPosition)
                transform.DOMove(originalPosition, animationTime).SetDelay(delay);


            if (lerpEulerAngles)
                transform.DORotate(originalEuler, animationTime).SetDelay(delay);


            if (lerpScale)
                transform.DOScale(originalScale, animationTime).SetDelay(delay);
        }

        hasLerped = !hasLerped;
    }
}
