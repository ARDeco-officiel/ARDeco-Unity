using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSounds : MonoBehaviour
{
    public void SoundsButtonClick()
    {
        SceneManager.LoadScene("Sounds");
    }
}
