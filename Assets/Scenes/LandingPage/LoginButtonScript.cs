using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginButtonScript : MonoBehaviour
{
    public void OnLoginButtonClick()
    {
        SceneManager.LoadScene("Login");
    }
}
