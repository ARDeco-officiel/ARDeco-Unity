using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class EditProfilScript : MonoBehaviour
{
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField emailInput;
    public TMP_InputField phoneNumber;
    public TMP_InputField city;
    public Button editButton; // Bouton "Modifier le profil"
    public Button saveButton; // Bouton "Enregistrer" (initialement désactivé)

    private string originalFirstName;
    private string originalLastName;
    private string originalEmail;
    private string originalPhoneNumber;
    private string originalCity;

    private void Start()
    {
        string jwt = PlayerPrefs.GetString("jwt");
        int userID = PlayerPrefs.GetInt("userID");

        // Appelez la fonction de récupération du profil lorsque la scène démarre
        StartCoroutine(GetProfileRequest(jwt, userID.ToString()));

        // Associez la fonction EditProfile au bouton "Modifier le profil"
        editButton.onClick.AddListener(EditProfile);
        // Associez la fonction SaveProfile au bouton "Enregistrer" (initialement désactivé)
        saveButton.onClick.AddListener(SaveProfile);
    }

    public IEnumerator GetProfileRequest(string jwt, string userID)
    {
        string uri = "https://api.ardeco.app/user/" + userID;
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + jwt);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            foreach (var header in headers)
            {
                webRequest.SetRequestHeader(header.Key, header.Value);
            }

            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                Debug.LogError("Erreur lors de la récupération du profil : " + webRequest.error);
                Debug.LogError("Réponse : " + webRequest.downloadHandler.text);
            }
            else
            {
                Debug.Log("Récupération du profil réussie!");
                Debug.Log("Réponse : " + webRequest.downloadHandler.text);

                string jsonResponse = webRequest.downloadHandler.text;
                ProfileData profileData = JsonUtility.FromJson<ProfileData>(jsonResponse);

                // Mettez à jour les champs de texte avec les données du profil
                originalFirstName = profileData.firstname;
                originalLastName = profileData.lastname;
                originalEmail = profileData.email;
                originalPhoneNumber = profileData.phone;
                originalCity = profileData.city;

                firstName.text = originalFirstName;
                lastName.text = originalLastName;
                emailInput.text = originalEmail;
                phoneNumber.text = originalPhoneNumber;
                city.text = originalCity;

                // Désactivez l'interactivité des champs de texte
                DisableTextFields();
            }
        }
    }

    public void EditProfile()
    {
        // Activez l'interactivité des champs de texte pour permettre l'édition
        EnableTextFields();

        // Activez le bouton "Enregistrer"
        saveButton.gameObject.SetActive(true);

        // Désactivez le bouton "Modifier le profil" pour éviter d'appuyer dessus pendant l'édition
        editButton.interactable = false;
    }

    public void SaveProfile()
    {
        // Récupérez les données modifiées des champs de texte
        string newFirstName = firstName.text;
        string newLastName = lastName.text;
        string newEmail = emailInput.text;
        string newPhoneNumber = phoneNumber.text;
        string newCity = city.text;

        // Simulez l'envoi des modifications au serveur (affichez-les dans la console pour le moment)
        Debug.Log("Envoi des modifications au serveur...");
        Debug.Log("Nouveau prénom : " + newFirstName);
        Debug.Log("Nouveau nom de famille : " + newLastName);
        // ... (autres données)

        // Désactivez l'interactivité des champs de texte après l'envoi réussi
        DisableTextFields();

        // Désactivez le bouton "Enregistrer" après l'envoi réussi
        saveButton.gameObject.SetActive(false);

        // Réactivez le bouton "Modifier le profil"
        editButton.interactable = true;
    }

    private void DisableTextFields()
    {
        firstName.interactable = false;
        lastName.interactable = false;
        emailInput.interactable = false;
        phoneNumber.interactable = false;
        city.interactable = false;
    }

    private void EnableTextFields()
    {
        firstName.interactable = true;
        lastName.interactable = true;
        emailInput.interactable = true;
        phoneNumber.interactable = true;
        city.interactable = true;
    }
}

// Créez une classe pour mapper les données du profil depuis la réponse JSON
[System.Serializable]
public class ProfileData
{
    public string id;
    public string firstname;
    public string lastname;
    public string email;
    public string phone;
    public string city;
}