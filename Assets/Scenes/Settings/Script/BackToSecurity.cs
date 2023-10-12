using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSecurity : MonoBehaviour
{
    public void SecurityButtonClick()
    {
        SceneManager.LoadScene("Security");
    }
}

