using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System.Collections;

public class ProfilDeleteConfirmationScript : MonoBehaviour
{
    public GameObject confirmationPanel;
    public GameObject profilPanel;

    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        profilPanel.SetActive(false);
    }

    public void ConfirmDeletion()
    {
        // Récupérez le JWT et le User ID de PlayerPrefs
        string jwt = PlayerPrefs.GetString("jwt");
        int userID = PlayerPrefs.GetInt("userID");

        // Appelez la méthode de suppression de profil en envoyant le JWT et le User ID au serveur
        StartCoroutine(DeleteProfile(jwt, userID));
    }

    public IEnumerator DeleteProfile(string token, int userID)
    {
        string uri = "https://votre-api.com/deleteprofile"; // Remplacez par l'URL de votre endpoint de suppression de profil

        // Créez une requête HTTP pour supprimer le profil en envoyant le token et le User ID
        UnityWebRequest request = new UnityWebRequest(uri, "DELETE");
        request.SetRequestHeader("Authorization", "Bearer " + token);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
        {
            Debug.LogError("Erreur lors de la suppression de profil : " + request.error);
        }
        else
        {
            Debug.Log("Profil supprimé avec succès!");

            // Supprimez le token et le User ID de PlayerPrefs après la suppression réussie
            PlayerPrefs.DeleteKey("jwt");
            PlayerPrefs.DeleteKey("userID");

            // Redirigez l'utilisateur vers l'écran de connexion ou une autre scène appropriée
            SceneManager.LoadScene("Login");
        }

        // Cachez la boîte de dialogue de confirmation
        SceneManager.LoadScene("Login");
        HideConfirmationPanel();
    }

    public void HideConfirmationPanel()
    {
        confirmationPanel.SetActive(false);
        profilPanel.SetActive(true);
    }
}