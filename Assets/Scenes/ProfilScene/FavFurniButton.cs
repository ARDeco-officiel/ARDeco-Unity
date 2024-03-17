using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FavFurniButton : MonoBehaviour
{
    public void FavFurniOnClic()
    {
        SceneManager.LoadScene("FavFurniture");
    }
}