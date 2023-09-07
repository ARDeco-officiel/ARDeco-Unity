using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class ChangePasswordScript : MonoBehaviour
{
    public TMP_InputField currentPasswordInput;
    public TMP_InputField newPasswordInput;
    public TMP_InputField confirmPasswordInput;
    public TextMeshProUGUI errorText;

    public void ChangePassword()
    {
        // Vérifier si les champs sont vides
        if (string.IsNullOrEmpty(currentPasswordInput.text) ||
            string.IsNullOrEmpty(newPasswordInput.text) ||
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

        // Vérifier si le mot de passe actuel est correct
        if (!CheckCurrentPassword(currentPasswordInput.text))
        {
            ShowErrorMessage("Mot de passe actuel incorrect");
            return;
        }

        // Appeler la méthode ChangePasswordRequest pour mettre à jour le mot de passe
        StartCoroutine(ChangePasswordRequest(newPasswordInput.text));
    }

    private bool CheckCurrentPassword(string currentPassword)
    {
        // Implémentez ici la logique pour vérifier si le mot de passe actuel est correct
        // Vous pouvez utiliser une approche similaire à ce que vous avez dans votre méthode de connexion (loginRequest)
        // Cela dépendra de votre système d'authentification
        // Cette méthode doit retourner true si le mot de passe est correct, sinon false
        return true; // Remplacer par votre logique de vérification du mot de passe actuel
    }

    private IEnumerator ChangePasswordRequest(string newPassword)
    {
        string email = PlayerPrefs.GetString("email"); // Récupérez l'email de PlayerPrefs

        string uri = "https://api.ardeco.app/changepassword"; // Remplacez par l'URL de votre endpoint de changement de mot de passe

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("newPassword", newPassword);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                ShowErrorMessage("Erreur lors du changement de mot de passe : " + webRequest.error);
            }
            else
            {
                // Réinitialisez les champs et affichez un message de succès
                currentPasswordInput.text = string.Empty;
                newPasswordInput.text = string.Empty;
                confirmPasswordInput.text = string.Empty;
                ShowSuccessMessage("Mot de passe changé avec succès");

                UnityEngine.Debug.Log("Changement de mot de passe réussi!");
                UnityEngine.Debug.Log("Réponse : " + webRequest.downloadHandler.text);
            }
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
