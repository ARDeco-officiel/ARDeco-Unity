using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToHelpAndSupport : MonoBehaviour
{
    public void HelpAndSupportButtonClick()
    {
        SceneManager.LoadScene("HelpAndSupport");
    }
}