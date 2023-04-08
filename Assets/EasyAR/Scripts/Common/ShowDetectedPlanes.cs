using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShowDetectedPlanes : MonoBehaviour
{
    /// <summary>
    /// ARPlaneManager
    /// </summary>
    ARPlaneManager m_plane;
    public bool planeEnable = false;

    /// <summary>
    /// Access the Shadow plane
    /// </summary>
    GameObject shadowPlane;


    // Start is called before the first frame update
    void Awake()
    {
        m_plane = FindObjectOfType<ARPlaneManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (shadowPlane == null)
        {
            shadowPlane = GameObject.FindWithTag("ShadowPlane");
        }

        if (planeEnable)
        {
            SetAllPlanesActive(true);
            if (shadowPlane != null)
            {
                shadowPlane.SetActive(false);
            }
            
        }
        else
        {
            SetAllPlanesActive(false);
            
            if (shadowPlane != null)
            {
                shadowPlane.SetActive(true);
            }
        }
    }

    void SetAllPlanesActive(bool value)
    {
        foreach (var plane in m_plane.trackables)
            plane.gameObject.SetActive(value);
    }

    public void ShowHidePlanes()
    {
        planeEnable = !planeEnable;
    }
}
