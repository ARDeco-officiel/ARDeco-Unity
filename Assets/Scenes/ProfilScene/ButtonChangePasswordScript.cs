using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonChangePasswordScript : MonoBehaviour
{
    public void OnChangePasswordClick()
    {
        SceneManager.LoadScene("ChangePassword");
    }
}
