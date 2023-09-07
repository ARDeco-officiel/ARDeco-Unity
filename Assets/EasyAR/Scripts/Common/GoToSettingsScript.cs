using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToSettingsScript : MonoBehaviour
{
    public void OnSettingsButtonClick()
    {
        SceneManager.LoadScene("Settings");
    }
}
