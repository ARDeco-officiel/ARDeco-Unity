using UnityEngine;
using UnityEngine.UI;
using System.IO;
using TMPro;
using static System.Net.Mime.MediaTypeNames;

public class ProfileScript : MonoBehaviour
{
    public UnityEngine.UI.Image profileImage;
    [SerializeField]
    private TMP_InputField firstName;

    [SerializeField]
    private TMP_InputField lastName;

    [SerializeField]
    private TMP_InputField emailInput;

    [SerializeField]
    private TMP_InputField phoneNumber;

    [SerializeField]
    private TMP_InputField city;

    private void Start()
    {
        firstName.text = "John";
        lastName.text = "Doe";
        emailInput.text = "johndoe@example.com";
        phoneNumber.text = "1234567890";
        city.text = "Paris";
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
}