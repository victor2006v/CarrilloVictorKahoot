using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;

public class UserCreator : MonoBehaviour {
    [SerializeField] private TMP_InputField userField;
    private string userPathFile;

    private XmlSerializer serializer = new XmlSerializer(typeof(UserList));

    private void Awake() {
        try
        {
            if (userField == null)
            {
                ErrorLogger.LogWarning("UserCreator", "User Input is missing");
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
        catch (Exception e)
        {
            ErrorLogger.LogException("UserCreator.Awake", e);
        }
    }

    private void Start() {
        userField.onEndEdit.AddListener(SaveUser);
    }

    private void OnDisable() {
        userField.onEndEdit.RemoveListener(SaveUser);
    }

    private void SaveUser(string inputName) {
        try
        {
            // First of all clean the input
            inputName = inputName.Trim();

            // If the user is empty
            if (string.IsNullOrEmpty(inputName))
            {
                ErrorLogger.LogWarning("UserCreator", "Attempted to save empty username");
                return;
            }

            UserList userList = new UserList();

            // If the file exists, load it
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
                    ErrorLogger.LogException("UserCreator.SaveUser[LoadExisting]", e);
                    Debug.LogError("XML File error. Creating a new list. Error: " + e.Message);
                    userList = new UserList();
                }
            }

            // Check if the user already exists
            bool userExists = userList.users.Any(u => u.userName.Equals(inputName, StringComparison.OrdinalIgnoreCase));
            if (userExists)
            {
                ErrorLogger.LogWarning("UserCreator", "User '" + inputName + "' already exists");
                Debug.LogWarning("User '" + inputName + "' already exists! Aborting save.");
                return;
            }

            // Add and Save
            userList.users.Add(new UserData { userName = inputName });

            using (FileStream fs = new FileStream(userPathFile, FileMode.Create))
            {
                serializer.Serialize(fs, userList);
            }

            Debug.Log("User '" + inputName + "' saved to XML.");
            PlayerPrefs.SetString("CurrentUser", inputName);
            PlayerPrefs.Save();
            Debug.Log("User '" + inputName + "' set as current user.");

            // Clear after a success user save
            userField.text = "";
        }
        catch (Exception e)
        {
            ErrorLogger.LogException("UserCreator.SaveUser", e);
        }
    }
}