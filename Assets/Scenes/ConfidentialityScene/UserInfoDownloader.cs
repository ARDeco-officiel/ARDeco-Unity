using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UserInfoDownloader : MonoBehaviour
{
    public void OnDownloadButtonClicked()
    {
        string userInfo = GenerateUserInfo(); // Replace this
        string fileName = "user_info.txt";

        string filePath = Path.Combine(Application.persistentDataPath, "user_info.txt");
        Debug.Log("File path: " + filePath);

        try
        {
            File.WriteAllText(filePath, userInfo);
            Debug.Log("File downloaded successfully");
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to download file: " + e.Message);
        }
    }

    // This method should be replaced with your actual method to generate user info in the desired format
    private string GenerateUserInfo()
    {
        string firstName = "John";
        string lastName = "Doe";
        string emailInput = "johndoe@example.com";
        string phoneNumber = "1234567890";
        string city = "Paris";
        // Format the user info in a desired way, for example:
        return "Name: " + firstName + " " + lastName + "\n" +
            "Email: " + emailInput + "\n" +
            "Phone number: " + phoneNumber + "\n" +
            "City: " + city;
    }
}