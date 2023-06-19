using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            errorText.text = "Veuillez remplir tous les champs";
            errorText.color = Color.red;
            errorText.gameObject.SetActive(true);
            return;
        }

        // Vérifier si les mots de passe correspondent
        if (newPasswordInput.text != confirmPasswordInput.text)
        {
            errorText.text = "Les mots de passe ne correspondent pas";
            errorText.color = Color.red;
            errorText.gameObject.SetActive(true);
            return;
        }

        // Vérifier si le mot de passe actuel est correct
        if (!CheckCurrentPassword(currentPasswordInput.text))
        {
            errorText.text = "Mot de passe actuel incorrect";
            errorText.color = Color.red;
            errorText.gameObject.SetActive(true);
            return;
        }

        // Mettre à jour le mot de passe dans la base de données
        UpdatePassword(newPasswordInput.text);

        // Réinitialiser les champs et afficher un message de succès
        currentPasswordInput.text = string.Empty;
        newPasswordInput.text = string.Empty;
        confirmPasswordInput.text = string.Empty;
        errorText.text = "Mot de passe changé avec succès";
        errorText.color = Color.green;
        errorText.gameObject.SetActive(true);
    }

    private bool CheckCurrentPassword(string currentPassword)
    {
        // Logique pour vérifier si le mot de passe actuel est correct
        // Vous devez implémenter cette logique en fonction de votre système de base de données ou d'authentification
        // Cette méthode doit retourner true si le mot de passe est correct, sinon false
        return true; // Remplacer par votre logique de vérification du mot de passe actuel
    }

    private void UpdatePassword(string newPassword)
    {
        // Logique pour mettre à jour le mot de passe dans la base de données
        // Vous devez implémenter cette logique en fonction de votre système de base de données ou d'authentification
    }
}
