using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerInfoScript : MonoBehaviour
{

    public GameObject reachable;
    public TMP_Text api;
    public TMP_Text version;
    public TMP_Text lastUpdate;
    public TMP_Text host;

    private double _lastPing = -3f;

    public void Update()
    {
        this.updateServerInfo();
    }

    public void updateServerInfo()
    {
        if (Time.time - _lastPing > 3f)
        {
            if (MultipleObjectPlacement.instance.status.reachable)
                reachable.GetComponent<Image>().color = new Color(.3f, .7f, .3f, 1);
            else
                reachable.GetComponent<Image>().color = Color.red;

            this.version.text = MultipleObjectPlacement.instance.status.version;
            this.api.text = MultipleObjectPlacement.instance.status.api;
            Debug.Log((MultipleObjectPlacement.instance.status.last_updtate));
            this.lastUpdate.text = MultipleObjectPlacement.instance.status.last_updtate.ToString();
            this.host.text = MultipleObjectPlacement.instance.status.host;
            _lastPing = Time.time;
        } 
    }
}
