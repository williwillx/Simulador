using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerifyFirstScene : MonoBehaviour
{
    public static VerifyFirstScene instance;

    public bool isOpened;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
