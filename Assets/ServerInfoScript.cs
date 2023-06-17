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
    public GameObject self;

    private double _lastPing = -3f;

    public void Update()
    {
        this.updateServerInfo();
    }

    public void updateServerInfo()
    {
        if (Time.time - _lastPing > 3f)
        {
            double timestamp = MultipleObjectPlacement.instance.status.last_update;
            DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = origin.AddMilliseconds(timestamp).ToLocalTime();

            if (MultipleObjectPlacement.instance.status.reachable)
                reachable.GetComponent<Image>().color = new Color(.3f, .7f, .3f, 1);
            else
                reachable.GetComponent<Image>().color = Color.red;

            this.version.text = MultipleObjectPlacement.instance.status.version;
            this.api.text = MultipleObjectPlacement.instance.status.api;
            this.lastUpdate.text = date.ToString("d/M/yy\nhh:mm:ss  ");
            this.host.text = MultipleObjectPlacement.instance.status.host;
            _lastPing = Time.time;
        } 
    }

    public void onCloseServerPanel(TMP_Text text)
    {
        if (text.text == "close")
        {
            text.text = "open";
            this.self.SetActive(false);
        }
        else
        {
            text.text = "close";
            self.SetActive(true);   
        }
    }
}
