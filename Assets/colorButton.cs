using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class colorButton : MonoBehaviour
{
    public int buttonIndex; // L'index du bouton

    public static colorButton instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    public void clickedButton()
    {
        materialMenuScript.instance.materialChangePrimary(buttonIndex);
    }

}
