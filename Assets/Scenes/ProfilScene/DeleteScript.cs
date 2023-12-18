using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class DeleteScript : MonoBehaviour
{
    public Button deleteButton; // Un bouton pour déclencher la suppression

    private void Start()
    {
        // Attachez une fonction de gestionnaire de clic au bouton de suppression
        deleteButton.onClick.AddListener(DeleteProfile);
    }

    private void DeleteProfile()
    {
        string jwt = PlayerPrefs.GetString("jwt");
        int userID = PlayerPrefs.GetInt("userID");

        // URL de l'endpoint de suppression du profil (à adapter à votre API)
        string deleteUri = "https://api.ardeco.app/close";

        // En-tête avec le jeton JWT
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + jwt);

        StartCoroutine(DeleteProfileRequest(deleteUri, headers));
    }

    private IEnumerator DeleteProfileRequest(string uri, Dictionary<string, string> headers)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.method = UnityWebRequest.kHttpVerbGET; // Définissez la méthode GET

            // Ajoutez les en-têtes à la requête
            foreach (var header in headers)
            {
                webRequest.SetRequestHeader(header.Key, header.Value);
            }

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError($"Erreur lors de la suppression du profil (code {webRequest.responseCode}): {webRequest.error}");
                Debug.LogError($"Réponse : {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.Log("Suppression du profil réussie!");
                Debug.Log($"Réponse : {webRequest.downloadHandler.text}");

                // Effacez les données de l'utilisateur de PlayerPrefs
                PlayerPrefs.DeleteKey("jwt");
                PlayerPrefs.DeleteKey("userID");

                // Chargez une autre scène ou effectuez une autre action (par exemple, revenir à l'écran de connexion)
                SceneManager.LoadScene("Login");
            }
        }
    }
}