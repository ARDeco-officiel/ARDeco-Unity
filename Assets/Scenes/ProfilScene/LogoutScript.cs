using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class LogoutScript : MonoBehaviour
{
    public Button logoutButton;

    private void Start()
    {
        // Attachez cette méthode à votre bouton de déconnexion dans l'inspecteur Unity
        logoutButton.onClick.AddListener(Logout);
    }

    public void Logout()
    {
        // Récupérez le JWT stocké dans PlayerPrefs
        string jwt = PlayerPrefs.GetString("jwt");

        // Assurez-vous que le JWT existe
        if (!string.IsNullOrEmpty(jwt))
        {
            StartCoroutine(LogoutRequest(jwt));
        }
        else
        {
            Debug.LogWarning("JWT introuvable. L'utilisateur n'est probablement pas connecté.");
        }
    }

    private IEnumerator LogoutRequest(string jwt)
    {
        string logoutUrl = "https://api.ardeco.app/logout";

        using (UnityWebRequest webRequest = UnityWebRequest.Get(logoutUrl))
        {
            // Ajoutez le jeton JWT à l'en-tête de la requête
            webRequest.SetRequestHeader("Authorization", "Bearer " + jwt);

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Erreur lors de la déconnexion : " + webRequest.error);
                Debug.LogError("Réponse : " + webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log("Déconnexion réussie!");

                // Supprimez le JWT stocké
                PlayerPrefs.DeleteKey("jwt");

                // Redirigez l'utilisateur vers l'écran de connexion
                SceneManager.LoadScene("Login");
            }
        }
    }
}