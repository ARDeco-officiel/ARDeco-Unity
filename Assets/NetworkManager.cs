using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using UnityEngine.UI;


public class NetworkManager : MonoBehaviour
{
	public static NetworkManager instance;
    private string ipServer = "api.ardeco.app"; // 54.37.14.208 & 127.0.0.1
    private string portServer = "";
    public GameObject infoPage;

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
        public double last_update;
    }


    [System.Serializable]
    public class FurnitureWrapper
    {
        public Furniture[] items;
    }

    [System.Serializable]
    public class Furniture
    {
        public int id;
        public string name;
        public int price;
        public string styles;
        public string rooms;
        public int width;
        public int height;
        public int depth;
        public string colors;
        public string object_id;
        public string model_id;
        public bool active;
        public int company;
        public string company_name;
    }

    public List<Furniture> filteredCatalog = new List<Furniture>();
    public List<Furniture> allCatalog = new List<Furniture>();
    public TMP_Text priceFilter;
    public TMP_Text styleFilter;
    public TMP_Text roomFilter;
    public TMP_Text ColorFilter;
 //   public GameObject typeFilter;
  //  public GameObject brandFilter;




/************************************************************ Server Requests ***********************************************************/

    public IEnumerator GetServerStatus()
    {
		string uri = "https://" + ipServer + portServer + "/status";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                ServerStatus tmp = JsonUtility.FromJson<ServerStatus>(webRequest.downloadHandler.text);
                Debug.Log(tmp.last_update + tmp.reachable.ToString());
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

    public IEnumerator GetCatalogue()
    {
		string uri = "https://" + ipServer + portServer + "/catalog";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                
                string tmp = "\"items:\"" + webRequest.downloadHandler.text;
                FurnitureWrapper furnitureWrapper = JsonUtility.FromJson<FurnitureWrapper>("{\"items\":" + webRequest.downloadHandler.text + "}");
                allCatalog = furnitureWrapper.items.ToList();
                Furniture[] furnitureArray = furnitureWrapper.items;

                foreach(Furniture furniture in furnitureArray)
                {
                    Debug.Log("Name: " + furniture.name);
                }
                infoPage.GetComponent<infoItem>().name.text = furnitureArray[0].name;
                infoPage.GetComponent<infoItem>().dimensions.text = furnitureArray[0].height.ToString() + " x " +  furnitureArray[0].width.ToString()  + " x " +  furnitureArray[0].depth.ToString();
                infoPage.GetComponent<infoItem>().brand.text = furnitureArray[0].company_name;
                infoPage.GetComponent<infoItem>().price.text = furnitureArray[0].price.ToString();
                
            }
            else
            {
                Debug.Log(webRequest.error);
                Debug.Log("Error on Get Catalog : " + webRequest.downloadHandler.text);
            }
        }
    }

    public IEnumerator ApplyFilter() {
        string LimitPrice = priceFilter.text;
        int LimitPriceInt;
        if (LimitPrice == "Pas de limite de prix") {
            LimitPriceInt = 10000000;
            
        } else {
            LimitPrice = LimitPrice.Replace(" ", "");
            LimitPrice = LimitPrice.Replace("€", "");
            LimitPriceInt = Int32.Parse(LimitPrice);
        }
        string style = styleFilter.text.ToLower();
        string room = roomFilter.text.ToLower();
        string color = ColorFilter.text.ToLower();
        int brandID = 1;
        int BestScore = 0;

        string uri = "https://" + ipServer + portServer + "/catalog";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                
                string tmp = "\"items:\"" + webRequest.downloadHandler.text;
                FurnitureWrapper furnitureWrapper = JsonUtility.FromJson<FurnitureWrapper>("{\"items\":" + webRequest.downloadHandler.text + "}");
                Furniture[] furnitureArray = furnitureWrapper.items;

                foreach(Furniture furniture in furnitureArray)
                {
                    int score = 0;
                    Debug.Log("Name: " + furniture.name);
                    Debug.Log("Price: " + furniture.price + " Limit: " + LimitPriceInt);
                    Debug.Log("Style: " + furniture.styles);
                    Debug.Log("Room: " + furniture.rooms);
                    Debug.Log("Brand: " + furniture.company);
    
            
                    if (furniture.price <= LimitPriceInt) score++;
                    if (furniture.rooms.ToLower().Contains(room)) score++;
                    if (furniture.styles.ToLower().Contains(style)) score++;
                    if (furniture.colors.ToLower().Contains(color)) score++;
                    if (furniture.company == brandID) score++;
                    Debug.Log("Score for " + furniture.name + " : " + score);
                    if (score > BestScore) {
                        filteredCatalog.Clear();
                        filteredCatalog.Add(furniture);
                        BestScore = score;
                    } else if (score == BestScore) { 
                        Debug.Log("Same score for " + furniture.name + " : " + score);
                        filteredCatalog.Add(furniture);
                    }
                }
                foreach(Furniture furniture2 in filteredCatalog)
                {
                    Debug.Log("Name selected : " + furniture2.name);
                }
            }
            else
            {
                Debug.Log(webRequest.error);
                Debug.Log("Error on Get Catalog : " + webRequest.downloadHandler.text);
            }
        }
    }
}
