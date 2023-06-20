using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Android;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

public class DownloadDataScript : MonoBehaviour
{
    public void OnDownloadButtonClick()
    {
        UserData userData = GetUserData();

        string jsonData = JsonUtility.ToJson(userData, true);
        string saveDirectory = Path.Combine(UnityEngine.Application.persistentDataPath, "Downloads");
        Directory.CreateDirectory(saveDirectory);

        string savePath = Path.Combine(saveDirectory, "user_data.json");

        File.WriteAllText(savePath, jsonData);

        OpenFileForDownload(savePath);
    }

    public UserData GetUserData()
    {
        UserData userData = new UserData();
        userData.firstName = "John";
        userData.lastName = "Doe";
        userData.email = "johndoe@example.com";
        userData.phone = "1234567890";
        userData.city = "Paris";

        return userData;
    }

    public void OpenFileForDownload(string filePath)
    {
#if UNITY_ANDROID
        AndroidJavaClass unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
        string intentAction = intentClass.GetStatic<string>("ACTION_VIEW");
        AndroidJavaObject intent = new AndroidJavaObject("android.content.Intent", intentAction);
        AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
        AndroidJavaObject uri = uriClass.CallStatic<AndroidJavaObject>("parse", "file://" + filePath);
        intent.Call<AndroidJavaObject>("setDataAndType", uri, "application/json");
        intent.Call<AndroidJavaObject>("addFlags", intentClass.GetStatic<int>("FLAG_GRANT_READ_URI_PERMISSION"));
        currentActivity.Call("startActivity", intent);
#endif
    }
}

[System.Serializable]
public class UserData
{
    public string firstName;
    public string lastName;
    public string email;
    public string phone;
    public string city;
}

//rajouter la ligne <uses-permission android:name="android.permission.WRITE_EXTERNAL_STORAGE" /> dans l'AndroidManifest.xml