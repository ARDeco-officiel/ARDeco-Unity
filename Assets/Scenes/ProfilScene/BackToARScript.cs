using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToARScript : MonoBehaviour
{
    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("ARMultipleObjects");
    }
}

