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
using Unity.VisualScripting;


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

        
    [Serializable]
    public class Furniture
    {
        public int id;
        public string name;
        public float price;
        public string styles;
        public string rooms;
        public float width;
        public float height;
        public float depth;
        public string colors;
        public string object_id;
        public int model_id;
        public bool active;
        public int company;
        public string company_name;
    }

    [Serializable]
    public class Response
    {
        public string status;
        public int code;
        public string description;
        public List<Furniture> data;
    }

    [Serializable]
    public class RootObject
    {
        public Response response;
    }

    public class ObjectDetails
    {
        public int price;
        public string color;
        public string room;
        public string style;
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
            string jsonResponse = webRequest.downloadHandler.text;
    Debug.Log("JSON Response: " + jsonResponse);
            Response rootObject = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);

            if (rootObject != null && rootObject != null && rootObject.data != null)
            {
                Furniture[] furnitureArray = rootObject.data.ToArray();
                allCatalog = rootObject.data;
                foreach (Furniture furniture in furnitureArray)
                {

                    Debug.Log("Name: " + furniture.name);
                }
                infoPage.GetComponent<infoItem>().name.text = furnitureArray[0].name;
                infoPage.GetComponent<infoItem>().dimensions.text = furnitureArray[0].height.ToString() + " x " +  furnitureArray[0].width.ToString()  + " x " +  furnitureArray[0].depth.ToString();
                infoPage.GetComponent<infoItem>().brand.text = furnitureArray[0].company_name;
                infoPage.GetComponent<infoItem>().price.text = furnitureArray[0].price.ToString();
                
            } else {
                Debug.LogError("rootObject, rootObject.response, or rootObject.response.data is null");
            }
        }
        else
        {
            Debug.LogError(webRequest.error);
            Debug.LogError("Error on Get Catalog : " + webRequest.downloadHandler.text);
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
                
//                FurnitureWrapper furnitureWrapper = JsonUtility.FromJson<FurnitureWrapper>("{\"items\":" + webRequest.downloadHandler.text + "}");
                Response rootObject = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);

                Furniture[] furnitureArray = rootObject.data.ToArray();

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

    public void callOpenCatalogueWithFilters() {
        StartCoroutine(ApplyFilterforChangement());
    }     
    public IEnumerator ApplyFilterforChangement() {
        
        int BestScore = 0;

        string uri = "https://" + ipServer + portServer + "/catalog";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            if (webRequest.result == UnityWebRequest.Result.Success)
            {
                Response rootObject = JsonUtility.FromJson<Response>(webRequest.downloadHandler.text);
                Furniture[] furnitureArray = rootObject.data.ToArray();
                foreach(Furniture furniture in furnitureArray)
                {
                    int score = 0;
                    if (furniture.id == MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().id) score = -1000000;
                    if (furniture.price <= MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().price) score++;
                    if (furniture.rooms.ToLower().Contains(MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().room)) score++;
                    if (furniture.styles.ToLower().Contains(MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().style)) score++;
                    if (furniture.colors.ToLower().Contains(MultipleObjectPlacement.instance._lastObjectTouched.GetComponent<SpawningObjectDetails>().color)) score++;
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
                int randomItem = UnityEngine.Random.Range(0, MultipleObjectPlacement.instance.ModelsList.Count);
               MultipleObjectPlacement.instance.spawnAtPosition(MultipleObjectPlacement.instance.ModelsList[randomItem],  filteredCatalog[0].id, Convert.ToInt32(filteredCatalog[0].price), filteredCatalog[0].colors, filteredCatalog[0].rooms, filteredCatalog[0].styles);

            }
            else
            {
                Debug.Log(webRequest.error);
                Debug.Log("Error on Get Catalog : " + webRequest.downloadHandler.text);
            }
        }
    }
}
