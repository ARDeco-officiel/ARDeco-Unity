using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToUpdates : MonoBehaviour
{
    public void UpdatesButtonClick()
    {
        SceneManager.LoadScene("Updates");
    }
}
