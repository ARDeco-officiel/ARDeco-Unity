using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToProfileScript : MonoBehaviour
{
    public void OnBackButtonClick()
    {
        SceneManager.LoadScene("Profil");
    }
}
