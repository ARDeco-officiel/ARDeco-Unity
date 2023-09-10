using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

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
            string simulatedResponse = SimulateLogout();

            // Traitez la réponse simulée comme si elle venait du serveur
            LogoutResponse response = JsonUtility.FromJson<LogoutResponse>(simulatedResponse);

            // Affichez un message à l'utilisateur
            Debug.Log("Déconnexion réussie : " + response.description);

            // Supprimez le JWT stocké
            PlayerPrefs.DeleteKey("jwt");

            // Redirigez l'utilisateur vers l'écran de connexion
            SceneManager.LoadScene("Login");
        }
        else
        {
            Debug.LogWarning("JWT introuvable. L'utilisateur n'est probablement pas connecté.");
        }
    }

    [System.Serializable]
    public class LogoutResponse
    {
        public string status;
        public int code;
        public string description;
    }

    public string SimulateLogout()
    {
        // Simulez une réponse JSON du serveur
        LogoutResponse response = new LogoutResponse
        {
            status = "OK",
            code = 200,
            description = "User has been successfully logged out"
        };

        // Convertissez la réponse en JSON
        string jsonResponse = JsonUtility.ToJson(response);

        return jsonResponse;
    }
}
