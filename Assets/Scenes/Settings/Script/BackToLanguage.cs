using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToLanguage : MonoBehaviour
{
    public void LanguageButtonClick()
    {
        SceneManager.LoadScene("Language");
    }
}