using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToConfidentialityScript : MonoBehaviour
{
    public void OnConfidentialityButtonClick()
    {
        SceneManager.LoadScene("Confidentiality");
    }
}