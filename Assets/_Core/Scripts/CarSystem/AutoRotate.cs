using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    [SerializeField] Vector3 rotationDirection;
    [HideInInspector] public bool shouldRun = true;

    private void Update()
    {
        if (shouldRun)
            transform.Rotate(rotationDirection * Time.deltaTime);
    }

    public void ToggleShouldRun()
    {
        shouldRun = !shouldRun;
    }
}
