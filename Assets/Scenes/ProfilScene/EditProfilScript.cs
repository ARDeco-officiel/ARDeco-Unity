using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using TMPro;
using System.Collections;

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
        // Sauvegarde les valeurs initiales des champs
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
            SaveChanges(); // Sauvegarde les modifications lorsque vous appuyez sur "Confirmer"
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
        // Récupére les nouvelles valeurs des champs de texte
        string newFirstName = firstNameField.text;
        string newLastName = lastNameField.text;
        string newEmail = emailField.text;
        string newPhoneNumber = phoneNumberField.text;
        string newCity = cityField.text;

        StartCoroutine(SendProfileChanges(newFirstName, newLastName, newEmail, newPhoneNumber, newCity));
    }

    private IEnumerator SendProfileChanges(string newFirstName, string newLastName, string newEmail, string newPhoneNumber, string newCity)
    {
        int userID = PlayerPrefs.GetInt("userID");
        string apiUrl = $"https://api.ardeco.app/user/{userID}";

        // Prépare les données à envoyer sous forme de chaîne JSON
        string jsonData = JsonUtility.ToJson(new UserProfileData(newFirstName, newLastName, newEmail, newPhoneNumber, newCity));

        // Envoyez la requête PUT
        using (UnityWebRequest webRequest = UnityWebRequest.Put(apiUrl, jsonData))
        {
            string jwt = PlayerPrefs.GetString("jwt");
            webRequest.method = "PUT";
            webRequest.SetRequestHeader("Authorization", "Bearer " + jwt);
            webRequest.SetRequestHeader("Content-Type", "application/json");

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError($"Erreur lors de la modification du profil (code {webRequest.responseCode}): {webRequest.error}");
                Debug.LogError($"Réponse : {webRequest.downloadHandler.text}");
            }
            else
            {
                Debug.Log("Modifications du profil enregistrées avec succès!");
            }
        }
    }

    [System.Serializable]
    public class UserProfileData
    {
        public string first_name;
        public string last_name;
        public string email;
        public string phone;
        public string city;

        public UserProfileData(string firstName, string lastName, string email, string phone, string city)
        {
            this.first_name = firstName;
            this.last_name = lastName;
            this.email = email;
            this.phone = phone;
            this.city = city;
        }
    }
}