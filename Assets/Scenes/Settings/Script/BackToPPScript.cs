using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToPPScript : MonoBehaviour
{
    public void PPButtonClick()
    {
        SceneManager.LoadScene("PrivacyPolicy");
    }
}

