using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTermOfUse : MonoBehaviour
{
    public void TermOfUseButtonClick()
    {
        SceneManager.LoadScene("TermOfUse");
    }
}
