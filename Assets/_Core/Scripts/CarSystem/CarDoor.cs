using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CarDoor : MonoBehaviour
{
    bool isOpen;
    [SerializeField] float animationTime;

    [Header("Euler Angles config")]
    [SerializeField] Vector3 finalEulerAngles;

    [Header("Delay Config")]
    [SerializeField] float delay;

    private void Start()
    {
        isOpen = !(transform.eulerAngles == Vector3.zero);
    }

    public void ToggleDoor()
    {
        if (isOpen)
        {
            CloseDoor();
        } else
        {
            OpenDoor();
        }
    }

    public void OpenDoor(float _animTime = 2, float _delay = 0)
    {
        transform.DORotate(finalEulerAngles, _animTime).SetDelay(_delay);
        isOpen = true;
    }

    public void CloseDoor(float _animTime = 2, float _delay = 0)
    {
        transform.DORotate(Vector3.zero, _animTime).SetDelay(_delay);
        isOpen = false;
    }
}
