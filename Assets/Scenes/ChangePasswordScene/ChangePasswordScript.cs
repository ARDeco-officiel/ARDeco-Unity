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
        // V�rifier si les champs sont vides
        if (string.IsNullOrEmpty(currentPasswordInput.text) ||
            string.IsNullOrEmpty(newPasswordInput.text) ||
            string.IsNullOrEmpty(confirmPasswordInput.text))
        {
            errorText.text = "Veuillez remplir tous les champs";
            errorText.color = Color.red;
            errorText.gameObject.SetActive(true);
            return;
        }

        // V�rifier si les mots de passe correspondent
        if (newPasswordInput.text != confirmPasswordInput.text)
        {
            errorText.text = "Les mots de passe ne correspondent pas";
            errorText.color = Color.red;
            errorText.gameObject.SetActive(true);
            return;
        }

        // V�rifier si le mot de passe actuel est correct
        if (!CheckCurrentPassword(currentPasswordInput.text))
        {
            errorText.text = "Mot de passe actuel incorrect";
            errorText.color = Color.red;
            errorText.gameObject.SetActive(true);
            return;
        }

        // Mettre � jour le mot de passe dans la base de donn�es
        UpdatePassword(newPasswordInput.text);

        // R�initialiser les champs et afficher un message de succ�s
        currentPasswordInput.text = string.Empty;
        newPasswordInput.text = string.Empty;
        confirmPasswordInput.text = string.Empty;
        errorText.text = "Mot de passe chang� avec succ�s";
        errorText.color = Color.green;
        errorText.gameObject.SetActive(true);
    }

    private bool CheckCurrentPassword(string currentPassword)
    {
        // Logique pour v�rifier si le mot de passe actuel est correct
        // Vous devez impl�menter cette logique en fonction de votre syst�me de base de donn�es ou d'authentification
        // Cette m�thode doit retourner true si le mot de passe est correct, sinon false
        return true; // Remplacer par votre logique de v�rification du mot de passe actuel
    }

    private void UpdatePassword(string newPassword)
    {
        // Logique pour mettre � jour le mot de passe dans la base de donn�es
        // Vous devez impl�menter cette logique en fonction de votre syst�me de base de donn�es ou d'authentification
    }
}
