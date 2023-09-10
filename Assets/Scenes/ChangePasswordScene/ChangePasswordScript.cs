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

        // Simuler la vérification du mot de passe actuel
        if (!SimulateCheckCurrentPassword(currentPasswordInput.text))
        {
            ShowErrorMessage("Mot de passe actuel incorrect");
            return;
        }

        // Appeler la méthode de changement de mot de passe
        StartCoroutine(ChangePasswordRequest(PlayerPrefs.GetString("jwt"), newPasswordInput.text));
    }

    // Simulation de la vérification du mot de passe actuel
    private bool SimulateCheckCurrentPassword(string currentPassword)
    {
        // Remplacez ceci par votre propre logique de vérification du mot de passe actuel côté serveur
        // Pour la simulation, nous utilisons un mot de passe fictif
        string simulatedCurrentPassword = "motdepasse123";
        return currentPassword == simulatedCurrentPassword;
    }

    private IEnumerator ChangePasswordRequest(string jwt, string newPassword)
    {
        string uri = "https://votre-api.com/changepassword"; // Remplacez par l'URL de votre endpoint de changement de mot de passe

        WWWForm form = new WWWForm();
        form.AddField("newPassword", newPassword);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            // Ajoutez le JWT aux headers de la requête
            webRequest.SetRequestHeader("Authorization", "Bearer " + jwt);

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
