using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BtnSceneChooser : MonoBehaviour
{
    [SerializeField] string sceneName;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(sceneName));
    }
}
