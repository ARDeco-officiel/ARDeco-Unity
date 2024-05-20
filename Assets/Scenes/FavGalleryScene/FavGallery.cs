using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class FavGallery : MonoBehaviour
{
    public string apiUrl = "https://api.ardeco.app/favorite/gallery";
    public TextMeshProUGUI galleryListText;
    public GameObject buttonPrefab;
    public Transform buttonContainer;

    private GalleryData galleryData;

    IEnumerator Start()
    {
        UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            galleryData = JsonUtility.FromJson<GalleryData>(json);

            string galleryList = "";
            foreach (var item in galleryData.data)
            {
                galleryList += "ID de la pièce : " + item.gallery.id + "\n";
                galleryList += "Nom : " + item.gallery.name + "\n";
                galleryList += "Description : " + item.gallery.description + "\n";
                galleryList += "Auteur : " + item.user.last_name + " " + item.user.first_name + "\n";
                galleryList += "Type de pièce : " + item.gallery.type + "\n";

                galleryList += "Meubles : ";
                foreach (var furniture in item.gallery.furniture)
                {
                    galleryList += furniture.id + ", ";
                }
                galleryList = galleryList.TrimEnd(',', ' ') + "\n\n";

                // Créer le bouton de suppression pour cette galerie
                GameObject deleteButton = Instantiate(buttonPrefab, buttonContainer);
                deleteButton.GetComponentInChildren<TextMeshProUGUI>().text = "Supprimer";
                string galleryId = item.gallery.id.ToString(); // Convertir l'ID en chaîne
                deleteButton.GetComponent<Button>().onClick.AddListener(() => DeleteGallery(galleryId));
            }

            galleryListText.text = galleryList;
        }
        else
        {
            Debug.LogError("Erreur lors de la récupération des données de l'API : " + request.error);
        }
    }

    void DeleteGallery(string galleryId)
    {
        StartCoroutine(DeleteGalleryCoroutine(galleryId));
    }

    IEnumerator DeleteGalleryCoroutine(string galleryId)
    {
        string deleteUrl = "https://api.ardeco.app/favorite/gallery/" + galleryId;
        UnityWebRequest request = UnityWebRequest.Delete(deleteUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // Trouver et supprimer la galerie de la liste de données
            for (int i = 0; i < galleryData.data.Length; i++)
            {
                if (galleryData.data[i].gallery.id.ToString() == galleryId)
                {
                    List<DataItem> newDataList = new List<DataItem>(galleryData.data);
                    newDataList.RemoveAt(i);
                    galleryData.data = newDataList.ToArray();

                    // Détruire le bouton de suppression correspondant
                    Destroy(buttonContainer.GetChild(i).gameObject);

                    break;
                }
            }

            // Rafraîchir la liste des galeries affichées
            RefreshGalleryList();
            Debug.Log("Galerie supprimée avec succès !");
        }
        else
        {
            Debug.LogError("Erreur lors de la suppression de la galerie : " + request.error);
        }
    }

    void RefreshGalleryList()
    {
        // Effacer le texte actuel
        galleryListText.text = "";

        string galleryList = "";
        foreach (var item in galleryData.data)
        {
            // Ajouter les détails de la galerie au texte
            galleryList += "ID de la pièce : " + item.gallery.id + "\n";
            galleryList += "Nom : " + item.gallery.name + "\n";
            galleryList += "Description : " + item.gallery.description + "\n";
            galleryList += "Auteur : " + item.user.last_name + " " + item.user.first_name + "\n";
            galleryList += "Type de pièce : " + item.gallery.type + "\n";

            galleryList += "Meubles : ";
            foreach (var furniture in item.gallery.furniture)
            {
                galleryList += furniture.id + ", ";
            }
            galleryList = galleryList.TrimEnd(',', ' ') + "\n\n";
        }

        // Mettre à jour le texte affiché
        galleryListText.text = galleryList;
    }

    [System.Serializable]
    public class Gallery
    {
        public int id;
        public string name;
        public string description;
        public string type;
        public Furniture[] furniture;
    }

    [System.Serializable]
    public class User
    {
        public int id;
        public string first_name;
        public string last_name;
    }

    [System.Serializable]
    public class GalleryData
    {
        public string status;
        public int code;
        public string description;
        public DataItem[] data;
    }

    [System.Serializable]
    public class DataItem
    {
        public Gallery gallery;
        public User user;
    }

    [System.Serializable]
    public class Furniture
    {
        public int id;
    }
}