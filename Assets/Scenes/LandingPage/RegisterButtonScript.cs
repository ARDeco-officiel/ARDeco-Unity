using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterButtonScript : MonoBehaviour
{
    public void OnRegisterButtonClick()
    {
        SceneManager.LoadScene("Register");
    }
}
