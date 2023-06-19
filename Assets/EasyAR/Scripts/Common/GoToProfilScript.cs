using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToProfilScript : MonoBehaviour
{
    public void OnProfilButtonClick()
    {
        SceneManager.LoadScene("Profil");
    }
}
