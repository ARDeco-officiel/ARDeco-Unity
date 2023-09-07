using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using System.Collections;
using TMPro;

public class ProfileScript : MonoBehaviour
{
    public UnityEngine.UI.Image profileImage;
    public TMP_InputField firstName;
    public TMP_InputField lastName;
    public TMP_InputField emailInput;
    public TMP_InputField phoneNumber;
    public TMP_InputField city;

    private void Start()
    {
        string email = PlayerPrefs.GetString("email");
        string password = PlayerPrefs.GetString("password");
        // Appelez la fonction de récupération du profil lorsque la scène démarre
        StartCoroutine(GetProfileRequest(email, password));
    }

    public IEnumerator GetProfileRequest(string email, string password)
    {
        string uri = "https://api.ardeco.app/user"; // Remplacez par l'URL de votre endpoint de récupération de profil
        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
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
                firstName.text = profileData.firstName;
                lastName.text = profileData.lastName;
                emailInput.text = profileData.email;
                phoneNumber.text = profileData.phoneNumber;
                city.text = profileData.city;
            }
        }
    }
}

// Créez une classe pour mapper les données du profil depuis la réponse JSON
[System.Serializable]
public class ProfileData
{
    public string firstName;
    public string lastName;
    public string email;
    public string phoneNumber;
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