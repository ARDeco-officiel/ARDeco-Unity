using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;


public class LoginScript : MonoBehaviour
{
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;

    private void Start()
    {
        emailInput = GameObject.Find("Email InputField").GetComponent<TMP_InputField>();
        passwordInput = GameObject.Find("Password InputField").GetComponent<TMP_InputField>();
    }

    public void OnLoginButtonClick()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        UnityEngine.Debug.Log("Username: " + email + ", Password: " + password);
        StartCoroutine(loginRequest(email, password));
    }

    public IEnumerator loginRequest(string email, string password)
    {
        string uri = "https://api.ardeco.app/login";

        WWWForm form = new WWWForm();
        form.AddField("email", email);
        form.AddField("password", password);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                UnityEngine.Debug.Log("Error on login : " + webRequest.error);
                UnityEngine.Debug.Log("Response : " + webRequest.downloadHandler.text);
            }
            else
            {
                UnityEngine.Debug.Log("Login successful!");
                UnityEngine.Debug.Log("Response : " + webRequest.downloadHandler.text);
                SceneManager.LoadScene("ARMultipleObjects");
            }
        }
    }
}