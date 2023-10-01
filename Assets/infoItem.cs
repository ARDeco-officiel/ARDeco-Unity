using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class infoItem : MonoBehaviour
{

    public TMP_Text name;
    public TMP_Text price;
    public TMP_Text brand;
    public TMP_Text dimensions;
    public GameObject miniature;

    public static infoItem instance;
    
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
