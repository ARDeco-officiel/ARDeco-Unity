using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLandingScript : MonoBehaviour
{
    public void OnDeconnectionButtonClick()
    {
        SceneManager.LoadScene("Landing");
    }
}