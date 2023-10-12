using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
public class EditProfilScript : MonoBehaviour
{
    public TMP_InputField firstNameField;
    public TMP_InputField lastNameField;
    public TMP_InputField emailField;
    public TMP_InputField phoneNumberField;
    public TMP_InputField cityField;

    public Button editProfileButton;
    private bool isEditing = false;

    private string initialFirstName;
    private string initialLastName;
    private string initialEmail;
    private string initialPhoneNumber;
    private string initialCity;

    private void Start()
    {
        // Sauvegardez les valeurs initiales des champs
        initialFirstName = firstNameField.text;
        initialLastName = lastNameField.text;
        initialEmail = emailField.text;
        initialPhoneNumber = phoneNumberField.text;
        initialCity = cityField.text;
        DisableEditing();
    }

    public void ToggleEditMode()
    {
        isEditing = !isEditing; // Inversez le mode d'édition

        // Changer le texte du bouton en conséquence
        if (isEditing)
        {
            editProfileButton.GetComponentInChildren<TMP_Text>().text = "Confirmer";
            EnableEditing();
        }
        else
        {
            editProfileButton.GetComponentInChildren<TMP_Text>().text = "Edit Profil";
            DisableEditing();
            SaveChanges(); // Sauvegardez les modifications lorsque vous appuyez sur "Confirmer"
        }
    }
    private void EnableEditing()
    {
        firstNameField.interactable = true;
        lastNameField.interactable = true;
        emailField.interactable = true;
        phoneNumberField.interactable = true;
        cityField.interactable = true;
    }

    private void DisableEditing()
    {
        firstNameField.interactable = false;
        lastNameField.interactable = false;
        emailField.interactable = false;
        phoneNumberField.interactable = false;
        cityField.interactable = false;
    }
    public void SaveChanges()
    {
        // Récupérez les nouvelles valeurs des champs de texte
        string newFirstName = firstNameField.text;
        string newLastName = lastNameField.text;
        string newEmail = emailField.text;
        string newPhoneNumber = phoneNumberField.text;
        string newCity = cityField.text;
        firstNameField.interactable = false;
        lastNameField.interactable = false;
        emailField.interactable = false;
        phoneNumberField.interactable = false;
        cityField.interactable = false;

        // Validez les données si nécessaire
        // Assurez-vous que les données sont correctes avant de les enregistrer.

        // Enregistrez les modifications dans le profil de l'utilisateur ou dans votre source de données
        // Vous pouvez utiliser PlayerPrefs ou une autre méthode de stockage de données.

        // Exemple d'utilisation de PlayerPrefs :
        PlayerPrefs.SetString("FirstName", newFirstName);
        PlayerPrefs.SetString("LastName", newLastName);
        PlayerPrefs.SetString("Email", newEmail);
        PlayerPrefs.SetString("PhoneNumber", newPhoneNumber);
        PlayerPrefs.SetString("City", newCity);
        PlayerPrefs.Save();
        Debug.Log("Modifications enregistrées!");
    }

}