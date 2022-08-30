using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AutoFade : MonoBehaviour
{
    CanvasGroup _canvasGroup;
    private void Awake()
    {
        _canvasGroup= gameObject.AddComponent<CanvasGroup>();
    }

    private void OnEnable()
    {
        _canvasGroup.alpha = 0;
        _canvasGroup.DOFade(1, .5f);
    }

}
