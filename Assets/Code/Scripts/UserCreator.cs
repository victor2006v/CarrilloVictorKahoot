using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class UserCreator : MonoBehaviour{
    [SerializeField] private TMP_InputField userField;
    private string userPathFile;

    private XmlSerializer serializer = new XmlSerializer(typeof(UserList));

    private void Awake(){
        if (userField == null) {
            Debug.LogWarning("User Input is missing");
            return;
        }
        string folderPath = Path.Combine(Application.persistentDataPath, "Users");

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
            Debug.Log("Created 'Users' directory at: " + folderPath);
        }

        userPathFile = Path.Combine(folderPath, "users.xml");
        Debug.Log("User file path set to: " + userPathFile);

    }
    private void Start(){
        userField.onEndEdit.AddListener(SaveUser);
    }
    private void OnDisable() {
        userField.onEndEdit.RemoveListener(SaveUser);
    }
    private void SaveUser(string inputName)
    {
        //First of all clean the input
        inputName = inputName.Trim();
        //If the user is empty
        if (string.IsNullOrEmpty(inputName)) return;

        UserList userList = new UserList();

        // If the file not exists
        if (File.Exists(userPathFile))
        {
            try
            {
                using (FileStream fs = new FileStream(userPathFile, FileMode.Open))
                {
                    userList = (UserList)serializer.Deserialize(fs);
                }
            }
            catch (Exception e)
            {
                Debug.LogError("XML File not found. Creating a new list. Error: " + e.Message);
                userList = new UserList(); // Reset list if file is broken
            }
        }
        //Checks if the user already exists
        bool userExists = userList.users.Any(u => u.userName.Equals(inputName, StringComparison.OrdinalIgnoreCase));
        if (userExists)
        {
            Debug.LogWarning($"User '{inputName}' already exists! Aborting save.");
            return; // Stop here, do not save
        }
        // 5. Add and Save
        userList.users.Add(new UserData { userName = inputName });

        using (FileStream fs = new FileStream(userPathFile, FileMode.Create))
        {
            serializer.Serialize(fs, userList);
        }

        Debug.Log($"User '{inputName}' saved to XML.");
        PlayerPrefs.SetString("CurrentUser", inputName);
        PlayerPrefs.Save();
        Debug.Log($"User '{inputName}' set as current user.");
        // Clear after a succes user save
        userField.text = "";
    }
}
