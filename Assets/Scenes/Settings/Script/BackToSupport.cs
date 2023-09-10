using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSupport : MonoBehaviour
{
    public void SupportButtonClick()
    {
        SceneManager.LoadScene("Support");
    }
}
