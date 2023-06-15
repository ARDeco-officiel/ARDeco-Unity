using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;

public class NetworkManager : MonoBehaviour
{
	public static NetworkManager instance;
    private string ipServer = "149.202.52.148"; // 149.202.52.148 & 127.0.0.1
    private string portServer = ":8080";

    public void Awake()
    {
        instance = this;
    }

    [System.Serializable]
    public class ServerStatus
    {
        public string api;
        public string version;
        public bool reachable;
        public string host;
        public double last_updtate;
    }
    
/************************************************************ Server Requests ***********************************************************/

    public IEnumerator GetServerStatus()
    {
		string uri = "http://" + ipServer + portServer + "/status";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
	            MultipleObjectPlacement.instance.status = JsonUtility.FromJson<ServerStatus>(webRequest.downloadHandler.text);
	            Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log(webRequest.error);
                Debug.Log("Error on GetUserRequest : " + webRequest.downloadHandler.text);
            }
        }
    }
}
