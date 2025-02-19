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

public class CharacterPage : MonoBehaviour
{

    [SerializeField] public TMP_InputField CharacterNameField;
    [SerializeField] public TMP_InputField CharacterTypeField;
    [SerializeField] public Button CharacterCreateButton;
    [SerializeField] public TMP_Dropdown SelectCharacterDropDown;
    [SerializeField] public TMP_Text SelectedCharacterText;
    [SerializeField] public TMP_Text WelcomeText;


    public static int userId;
    public static string userName;
    public static List<CharacterData> characterForTheLoggedInUser = new List<CharacterData>();
    public static CharacterData selectedCharacter;
    

    [System.Serializable]
    public class CharacterCreateData
    {
        public string name;
        public int lvl = 1;
        public int utokepesseg = 1;
        public int type;
        public int userId;
    }

    [System.Serializable]
    public class CharacterData
    {
        public int id;
        public string name;
        public int lvl;
        public int utokepesseg;
        public int type;
        public int userId;

        public CharacterData(int id, string name, int lvl, int utokepesseg, int type, int userId)
        {
            this.id = id;
            this.name = name;
            this.lvl = lvl;
            this.utokepesseg = utokepesseg;
            this.type = type;
            this.userId = userId;
        }
    }

    [System.Serializable]
    public class selectedID
    {
        public int ID = userId;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CharacterCreateButton.onClick.AddListener(CharacterCreateButtonPress);

        // Access the singleton instance
        if (MainPage.Instance != null)
        {
            //UnityEngine.Debug.Log(MainPage.Instance.instanceData);
            //UnityEngine.Debug.Log(MainPage.Instance.instanceData.Key + " - " + MainPage.Instance.instanceData.Value);
            userId = MainPage.Instance.instanceData.Value;
            userName = MainPage.Instance.instanceData.Key;



            UnityEngine.Debug.Log(Convert.ToString(MainPage.Instance.instanceData.Value) + "CREATION");
        }
        else
        {
            UnityEngine.Debug.LogError("MainPage instance not found!");
        }

        WelcomeText.text = String.Format($"Üdvözöllek durandában cigány({userName} {userId})!");

        selectedID tempid = new selectedID();

        string JSONdata = JsonUtility.ToJson(tempid);

        StartCoroutine(RefreshCharacters(JSONdata));
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CharacterCreateButtonPress()
    {
        if (characterForTheLoggedInUser.Count < 4)
        {
            CharacterCreateData characterCreateData = new CharacterCreateData
            {
                name = CharacterNameField.text,
                type = Convert.ToInt32(CharacterTypeField.text),
                userId = userId
            };

            string jsonData = JsonUtility.ToJson(characterCreateData);

            // Send the request to create a character
            StartCoroutine(SendCharacterCreateRequest(jsonData));
        }
        else
        {
            UnityEngine.Debug.Log("Maximum négy karaktert hozhatsz létre");
        }
    }

    private IEnumerator SendCharacterCreateRequest(string jsonData)
    {
        string url = "http://localhost:3000/characters/create";

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

            // Refresh the character list after creating the new character
            selectedID tempid = new selectedID();
            string JSONdata = JsonUtility.ToJson(tempid);
            StartCoroutine(RefreshCharacters(JSONdata));
        }
    }

    private IEnumerator RefreshCharacters(string jsondata)
    {
        string url = "http://localhost:3000/characters/get-all-characters-by-user";

        UnityWebRequest request = new UnityWebRequest(url, "GET")
        {
            uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsondata)),
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
            UnityEngine.Debug.Log($"All characters for userId {userId}: {responseText}");

            // Deserialize the JSON response into a CharacterDataList
            CharacterDataListWrapper characterDataList = JsonUtility.FromJson<CharacterDataListWrapper>("{\"characters\":" + responseText + "}");

            // Access the list of characters
            characterForTheLoggedInUser = characterDataList.characters;

            // Process the character data as needed
            foreach (var character in characterForTheLoggedInUser)
            {
                UnityEngine.Debug.Log($"Character Name: {character.name}, Level: {character.lvl}");
            }

            // Update the dropdown with the newly fetched list of characters
            PopulateDropdown();
        }
    }

    void PopulateDropdown()
    {
        List<string> characterNames = new List<string>();

        foreach (var character in characterForTheLoggedInUser)
        {
            characterNames.Add(character.name);
        }

        // Clear the current dropdown options and add the new ones
        SelectCharacterDropDown.ClearOptions();
        SelectCharacterDropDown.AddOptions(characterNames);

        // Optionally, set the selected character if one exists
        if (selectedCharacter != null)
        {
            int selectedIndex = characterForTheLoggedInUser.FindIndex(c => c.id == selectedCharacter.id);
            if (selectedIndex >= 0)
            {
                SelectCharacterDropDown.value = selectedIndex;
                SelectedCharacterText.text = selectedCharacter.name;
            }
        }

        // Add listener for selection change
        SelectCharacterDropDown.onValueChanged.AddListener(OnCharacterSelected);
    }

    void OnCharacterSelected(int index)
    {
        if (index >= 0 && index < characterForTheLoggedInUser.Count)
        {
            selectedCharacter = characterForTheLoggedInUser[index];
            UnityEngine.Debug.Log($"Selected Character: {selectedCharacter.name}");
            SelectedCharacterText.text = selectedCharacter.name;
        }
    }

    // Wrapper class to handle the list deserialization
    [System.Serializable]
    private class CharacterDataListWrapper
    {
        public List<CharacterData> characters;
    }
}
