using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FavFurniBackButton : MonoBehaviour
{
 public void backOnClic()
    {
        SceneManager.LoadScene("Profil");
    }
}