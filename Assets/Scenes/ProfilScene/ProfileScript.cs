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

    public void ChangeProfileImage()
    {
        string imagePath = OpenFilePicker();

        if (!string.IsNullOrEmpty(imagePath))
        {
            PlayerPrefs.SetString("ProfileImagePath", imagePath);

            LoadProfileImage();
        }
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

    private string OpenFilePicker()
    {

        string[] extensions = { "jpg", "jpeg", "png" };
        string path = UnityEditor.EditorUtility.OpenFilePanel("Sélectionner une image", "", string.Join(",", extensions));

        if (!string.IsNullOrEmpty(path))
        {
 
        }

        return path;
    }
}
