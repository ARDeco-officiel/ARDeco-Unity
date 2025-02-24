using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using TMPro;


public class RegisterScript : MonoBehaviour
{
    public TMP_InputField firstNameInput;
    public TMP_InputField lastNameInput;
    public TMP_InputField emailInput;
    public TMP_InputField passwordInput;
    public TMP_InputField confirmPasswordInput;
    public TMP_InputField phoneNumberInput;
    public TMP_InputField cityInput;

    private void Start()
    {
        firstNameInput = GameObject.Find("Firstname InputField").GetComponent<TMP_InputField>();
        lastNameInput = GameObject.Find("Lastname InputField").GetComponent<TMP_InputField>();
        emailInput = GameObject.Find("Email InputField").GetComponent<TMP_InputField>();
        passwordInput = GameObject.Find("Password InputField").GetComponent<TMP_InputField>();
        confirmPasswordInput = GameObject.Find("Confirm Password InputField").GetComponent<TMP_InputField>();
        phoneNumberInput = GameObject.Find("Phone Number InputField").GetComponent<TMP_InputField>();
        cityInput = GameObject.Find("City InputField").GetComponent<TMP_InputField>();
    }

    public void OnRegisterButtonClick()
    {
        string firstName = firstNameInput.text;
        string lastName = lastNameInput.text;
        string email = emailInput.text;
        string password = passwordInput.text;
        string confirmPassword = confirmPasswordInput.text;
        string phoneNumber = phoneNumberInput.text;
        string city = cityInput.text;

        UnityEngine.Debug.Log("Firstname: " + firstName + ", Lastname: " + lastName + ", Email: " + email + ", Password: " + password + ", Confirm Password: " + confirmPassword + ", Phone Number: " + phoneNumber + ", City: " + city);
        StartCoroutine(registerRequest(firstName, lastName, email, password, confirmPassword, phoneNumber, city));
    }

    public IEnumerator registerRequest(string firstName, string lastName, string email, string password, string confirmPassword, string phoneNumber, string city)
    {
        string uri = "https://api.ardeco.app/register";

        WWWForm form = new WWWForm();
        form.AddField("first_name", firstName);
        form.AddField("last_name", lastName);
        form.AddField("email", email);
        form.AddField("password", password);
        form.AddField("password_confirm", confirmPassword);
        form.AddField("phone", phoneNumber);
        form.AddField("city", city);

        using (UnityWebRequest webRequest = UnityWebRequest.Post(uri, form))
        {
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError || webRequest.isHttpError)
            {
                UnityEngine.Debug.Log("Error on register: " + webRequest.error);
                UnityEngine.Debug.Log("Response: " + webRequest.downloadHandler.text);
            }
            else
            {
                UnityEngine.Debug.Log("Register successful!");
                UnityEngine.Debug.Log("Response: " + webRequest.downloadHandler.text);
                SceneManager.LoadScene("Login");
            }
        }
    }
}







