using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sidePanelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (MultipleObjectPlacement.instance.getLastItem() != null)
        {
            this.gameObject.SetActive(true);
        }
        else
        {
            this.gameObject.SetActive(true);
        }
    }
}
