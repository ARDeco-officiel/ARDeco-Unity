using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;
using System.Collections.Generic;

public class ProfileScript : MonoBehaviour
{
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField emailInput;
    public TMP_InputField phoneNumber;
    public TMP_InputField city;

    private void Start()
    {
        string jwt = PlayerPrefs.GetString("jwt");
        int userID = PlayerPrefs.GetInt("userID");
        // Appelez la fonction de récupération du profil lorsque la scène démarre
        StartCoroutine(GetProfileRequest(jwt, userID.ToString()));
    }

    public IEnumerator GetProfileRequest(string jwt, string userID)
    {
        string uri = "https://api.ardeco.app/user/" + userID; // Remplacez par l'URL de votre endpoint de récupération de profil
        Dictionary<string, string> headers = new Dictionary<string, string>();
        headers.Add("Authorization", "Bearer " + jwt);

        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri)) // Utilisez Get pour récupérer le profil
        {
            // Ajoutez les headers à la requête
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

                // Parsez la réponse JSON pour extraire les données du profil
                string jsonResponse = webRequest.downloadHandler.text;
                ProfileData profileData = JsonUtility.FromJson<ProfileData>(jsonResponse);

                // Mettez à jour les champs de texte avec les données du profil
                firstName.text = profileData.firstname;
                lastName.text = profileData.lastname;
                emailInput.text = profileData.email;
                phoneNumber.text = profileData.phone;
                city.text = profileData.city;
                firstName.interactable = false;
                lastName.interactable = false;
                emailInput.interactable = false;
                phoneNumber.interactable = false;
                city.interactable = false;
                Debug.Log("Prénom : " + profileData.firstname);
            }
        }
    }
}

// Créez une classe pour mapper les données du profil depuis la réponse JSON
[System.Serializable]
public class ProfileData
{
    public string firstname;
    public string lastname;
    public string email;
    public string phone;
    public string city;
}
    /*public void ChangeProfileImage()
    {
        OpenFilePicker((imagePath) =>
        {
            if (!string.IsNullOrEmpty(imagePath))
            {
                PlayerPrefs.SetString("ProfileImagePath", imagePath);
                LoadProfileImage();
            }
        });
    }

    private void LoadProfileImage()
    {
        string profileImagePath = PlayerPrefs.GetString("ProfileImagePath");

        if (!string.IsNullOrEmpty(profileImagePath) && File.Exists(profileImagePath))
        {
            byte[] imageData = File.ReadAllBytes(profileImagePath);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(imageData);

            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.one * 0.5f);
            profileImage.sprite = sprite;
        }
    }

    private void OpenFilePicker(System.Action<string> callback)
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        string intentAction = intentClass.GetStatic<string>("ACTION_OPEN_DOCUMENT");

        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", intentAction);

        intent.Call<AndroidJavaObject>("setType", "image/*");
        intent.Call<AndroidJavaObject>("addCategory", intentClass.GetStatic<string>("CATEGORY_OPENABLE"));

        AndroidJavaObject chooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intent, "Select Image");
        AndroidJavaObject resultCallback = new AndroidJavaObject("com.example.FilePickerCallback", new FilePickerCallback(callback));

        currentActivity.Call("startActivityForResult", chooser, 0, resultCallback);
    }*/