using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowObjectOn(GameObject obj) {
        obj.SetActive(true);
    }

    public void ShowObjectOff(GameObject obj) {
        obj.SetActive(false);
    }
}
