using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using TMPro;

public class MainPage : MonoBehaviour
{
    public static MainPage Instance { get; private set; }

    [Header("Scene-Specific Data")]
    public KeyValuePair<string, int> instanceData = new KeyValuePair<string, int>("empty", -1);

    [SerializeField] public TMP_InputField UsernameField;
    [SerializeField] public TMP_InputField PasswordField;
    [SerializeField] public Button LoginButton;
    [SerializeField] public Button RegisterButton;
    [SerializeField] public Toggle RememberMeToggle;

    private string credentialsFilePath;

    [System.Serializable]
    public class LoginData
    {
        public string name;
        public string password;
    }

    [System.Serializable]
    public class ResponseData
    {
        public string name;
        public int ID;
    }

    void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Define the path to the credentials file
        credentialsFilePath = Path.Combine(Application.persistentDataPath, "credentials.txt");
    }

    void Start()
    {
        LoginButton.onClick.AddListener(LoginButtonPress);
        RegisterButton.onClick.AddListener(RegisterButtonPress);

        LoadCredentials();
    }

    public void RegisterButtonPress()
    {
        SceneManager.LoadScene("RegisterPage");
    }

    public void LoginButtonPress()
    {
        string username = UsernameField.text;
        string password = PasswordField.text;

        if (RememberMeToggle.isOn)
        {
            SaveCredentials(username, password);
        }
        else
        {
            DeleteCredentials();
        }

        // Proceed with the login process
        LoginData loginData = new LoginData
        {
            name = username,
            password = password
        };

        string jsonData = JsonUtility.ToJson(loginData);
        StartCoroutine(SendLoginRequest(jsonData));
    }

    private IEnumerator SendLoginRequest(string jsonData)
    {
        string url = "http://localhost:3000/users/login";

        UnityWebRequest request = new UnityWebRequest(url, "POST")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData)),
            downloadHandler = new DownloadHandlerBuffer()
        };

        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError($"Request error: {request.error}");
        }
        else
        {
            string responseText = request.downloadHandler.text;
            ResponseData data = JsonUtility.FromJson<ResponseData>(responseText);

            instanceData = new KeyValuePair<string, int>(data.name, data.ID);

            // Load the CharacterCreationPage scene asynchronously
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CharacterCreationPage");

            // Wait until the asynchronous scene fully loads
            while (!asyncLoad.isDone)
            {
                yield return null;
            }

            Debug.Log("CharacterCreationPage loaded successfully.");
            Debug.Log($"{instanceData.Key} ID: {instanceData.Value} MAINPAGE");
        }
    }

    private void SaveCredentials(string username, string password)
    {
        try
        {
            using (StreamWriter writer = new StreamWriter(credentialsFilePath))
            {
                writer.WriteLine(username);
                writer.WriteLine(password);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error saving credentials: {e.Message}");
        }
    }

    private void LoadCredentials()
    {
        try
        {
            if (File.Exists(credentialsFilePath))
            {
                using (StreamReader reader = new StreamReader(credentialsFilePath))
                {
                    string username = reader.ReadLine();
                    string password = reader.ReadLine();

                    UsernameField.text = username;
                    PasswordField.text = password;
                    RememberMeToggle.isOn = true;
                }
            }
            else
            {
                RememberMeToggle.isOn = false;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading credentials: {e.Message}");
        }
    }

    private void DeleteCredentials()
    {
        try
        {
            if (File.Exists(credentialsFilePath))
            {
                File.Delete(credentialsFilePath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error deleting credentials: {e.Message}");
        }
    }
}
