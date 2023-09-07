using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSettingsScript : MonoBehaviour
{
    public void SettingsButtonClick()
    {
        SceneManager.LoadScene("Settings");
    }
}
