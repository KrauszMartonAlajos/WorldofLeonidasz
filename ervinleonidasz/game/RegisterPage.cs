using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using System.Text;
using System.Diagnostics;
//using GlobalData;

public class RegisterPage : MonoBehaviour
{
    //login
    [SerializeField] public TMP_InputField UsernameField;
    [SerializeField] public TMP_InputField PasswordField;
    [SerializeField] public Button RegisterButton;
    [SerializeField] public Button BackButton;


    [System.Serializable]
    public class RegisterData
    {
        public string name;
        public string password;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        RegisterButton.onClick.AddListener(RegisterButtonPress);
        BackButton.onClick.AddListener(BackButtonPress);

    }

    // Update is called once per frame
    void Update()
    {
    }

    public void BackButtonPress()
    {
        SceneManager.LoadScene("LoginPage");
    }

    public void RegisterButtonPress()
    {
        //UnityEngine.Debug.LogError(GlobalData.playerID);

        RegisterData registerData = new RegisterData();

        registerData.name = UsernameField.text;
        registerData.password = PasswordField.text;

        string jsonData = JsonUtility.ToJson(registerData);

        StartCoroutine(SendRegisterRequest(jsonData));
    }

    private IEnumerator SendRegisterRequest(string jsonData)
    {
        string url = "http://localhost:3000/users/register";

        UnityWebRequest request = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            UnityEngine.Debug.LogError($"Request error: {request.error}");
        }
        else
        {
            string responseText = request.downloadHandler.text;
            UnityEngine.Debug.Log($"Response: {responseText}");
            SceneManager.LoadScene("LoginPage");

        }

        //localhost:3000/users/login
        //data: ID: null
        //name: UsernameField
        //password: PasswordField
    }
}
