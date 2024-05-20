using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI; 
using TMPro;
using System.Collections.Generic;

public class FavFurniture : MonoBehaviour
{
    public string apiUrl = "https://api.ardeco.app/favorite/furniture";
    public TextMeshProUGUI furnitureListText;
    public GameObject buttonPrefab; 
    public Transform buttonContainer; 

    private FurnitureData furnitureData;

    IEnumerator Start()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            furnitureData = JsonUtility.FromJson<FurnitureData>(json);

            string furnitureList = "";
            foreach (var item in furnitureData.data)
            {
                furnitureList += "Nom du meuble : " + item.furniture.name + "\n";
                furnitureList += "ID : " + item.furniture.id + "\n";
                furnitureList += "Prix : " + item.furniture.price + "\n";
                furnitureList += "Styles : " + item.furniture.styles + "\n";
                furnitureList += "Couleurs : " + item.furniture.colors + "\n";
                furnitureList += "Pièces : " + item.furniture.room + "\n";
                furnitureList += "Hauteur : " + item.furniture.height + "\n";
                furnitureList += "Largeur : " + item.furniture.width + "\n";
                furnitureList += "Profondeur : " + item.furniture.depth + "\n";
                furnitureList += "Entreprise : " + item.furniture.company + "\n\n";

                GameObject button = Instantiate(buttonPrefab, buttonContainer);
                button.GetComponentInChildren<TextMeshProUGUI>().text = "Supprimer";
                string furnitureId = item.furniture.id;
                button.GetComponent<Button>().onClick.AddListener(() => DeleteFurniture(furnitureId));
            }

            furnitureListText.text = furnitureList;
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des données de l'API : " + request.error);
        }
    }

void DeleteFurniture(string furnitureId)
    {
        StartCoroutine(DeleteFurnitureCoroutine(furnitureId));
    }

IEnumerator DeleteFurnitureCoroutine(string furnitureId)
    {
        string deleteUrl = "https://api.ardeco.app/favorite/furniture/" + furnitureId;
        UnityWebRequest request = UnityWebRequest.Delete(deleteUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            for (int i = 0; i < furnitureData.data.Length; i++)
            {
                if (furnitureData.data[i].furniture.id == furnitureId)
                {
                    // Supprimer l'élément de la liste de données
                    List<DataItem> newDataList = new List<DataItem>(furnitureData.data);
                    newDataList.RemoveAt(i);
                    furnitureData.data = newDataList.ToArray();
                    
                    // Supprimer le bouton correspondant de l'interface utilisateur
                    Destroy(buttonContainer.GetChild(i).gameObject);
                    
                    break;
                }
            }

            RefreshFurnitureList();
            Debug.Log("Meuble supprimé avec succès !");
        }
        else
        {
            Debug.LogError("Erreur lors de la suppression du meuble : " + request.error);
        }
    }

    void RefreshFurnitureList()
    {
        furnitureListText.text = "";

        string furnitureList = "";
        foreach (var item in furnitureData.data)
        {
            furnitureList += "Nom du meuble : " + item.furniture.name + "\n";
            furnitureList += "ID : " + item.furniture.id + "\n";
            furnitureList += "Prix : " + item.furniture.price + "\n";
            furnitureList += "Styles : " + item.furniture.styles + "\n";
            furnitureList += "Couleurs : " + item.furniture.colors + "\n";
            furnitureList += "Pièces : " + item.furniture.room + "\n";
            furnitureList += "Hauteur : " + item.furniture.height + "\n";
            furnitureList += "Largeur : " + item.furniture.width + "\n";
            furnitureList += "Profondeur : " + item.furniture.depth + "\n";
            furnitureList += "Entreprise : " + item.furniture.company + "\n\n";
        }
        furnitureListText.text = furnitureList;
    }

    [System.Serializable]
    public class Furniture
    {
        public string id;
        public string name;
        public float price;
        public string styles;
        public string colors;
        public string room;
        public float height;
        public float width;
        public float depth;
        public string company;
    }

    [System.Serializable]
    public class FavoriteFurniture
    {
        public int id;
        public int user_id;
        public string furniture_id;
        public string timestamp;
    }

    [System.Serializable]
    public class DataItem
    {
        public Furniture furniture;
        public FavoriteFurniture favorite_furniture;
    }

    [System.Serializable]
    public class FurnitureData
    {
        public string status;
        public int code;
        public string description;
        public DataItem[] data;
    }
}