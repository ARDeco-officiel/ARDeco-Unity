using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToAboutUsScript : MonoBehaviour
{
    public void AboutUsButtonClick()
    {
        SceneManager.LoadScene("AboutUs");
    }
}
