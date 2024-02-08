using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorButtonSecondary : MonoBehaviour
{
   public int buttonIndex; // L'index du bouton

    public static ColorButtonSecondary instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
    }

    public void clickedButton()
    {
        materialMenuScript.instance.materialChangeSecondary(buttonIndex);
    }
}
