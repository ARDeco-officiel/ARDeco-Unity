using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToFAQ : MonoBehaviour
{
    public void FAQButtonClick()
    {
        SceneManager.LoadScene("FAQ");
    }
}