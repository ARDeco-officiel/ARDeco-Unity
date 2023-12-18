using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class ChangePasswordScript : MonoBehaviour
{
    public TMP_InputField newPasswordInput;
    public TMP_InputField confirmPasswordInput;
    public TextMeshProUGUI errorText;

    public void ChangePassword()
    {
        // Vérifier si les champs sont vides
        if (string.IsNullOrEmpty(newPasswordInput.text) ||
            string.IsNullOrEmpty(confirmPasswordInput.text))
        {
            ShowErrorMessage("Veuillez remplir tous les champs");
            return;
        }

        // Vérifier si les mots de passe correspondent
        if (newPasswordInput.text != confirmPasswordInput.text)
        {
            ShowErrorMessage("Les mots de passe ne correspondent pas");
            return;
        }

        // Appeler la méthode de changement de mot de passe
        StartCoroutine(ChangePasswordRequest(newPasswordInput.text));
    }
    
    [System.Serializable]
    public class ChangePasswordData
    {
        public string password;
    }

    IEnumerator ChangePasswordRequest(string newPassword)
    {
        // Créer un objet JSON pour contenir le nouveau mot de passe
        int userID = PlayerPrefs.GetInt("userID");
        string uri = "https://api.ardeco.app/user/" + userID;
        ChangePasswordData requestData = new ChangePasswordData
        {
            password = newPassword
        };
        string jsonRequestBody = JsonUtility.ToJson(requestData);

        // Créer la requête PUT avec UnityWebRequest
        UnityWebRequest request = UnityWebRequest.Put(uri, jsonRequestBody);

        // Spécifier le type de contenu JSON
        request.SetRequestHeader("Content-Type", "application/json");

        // Envoyer la requête
        yield return request.SendWebRequest();
        Debug.Log("JSON Request Body: " + jsonRequestBody);
        Debug.LogError("Réponse du serveur : " + request.downloadHandler.text);
        // Vérifier s'il y a des erreurs
        if (request.result == UnityWebRequest.Result.Success)
        {
            ShowSuccessMessage("Mot de passe changé avec succès!");
            Debug.Log("Mot de passe changé avec succès!");
        }
        else
        {
            ShowErrorMessage("Erreur lors du changement de mot de passe");
            Debug.LogError("Erreur lors du changement de mot de passe: " + request.error);
        }
    }

    private void ShowErrorMessage(string message)
    {
        errorText.text = message;
        errorText.color = Color.red;
        errorText.gameObject.SetActive(true);
    }

    private void ShowSuccessMessage(string message)
    {
        errorText.text = message;
        errorText.color = Color.green;
        errorText.gameObject.SetActive(true);
    }
}
