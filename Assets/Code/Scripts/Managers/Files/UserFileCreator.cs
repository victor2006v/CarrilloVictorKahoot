using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class UserFileCreator : MonoBehaviour
{
    [SerializeField] private Button doneButton;
    [SerializeField] private NewUser newUserInput;

    private string filePath;
    private UserDatabase database;

    private void Awake() {
        filePath = Application.persistentDataPath + "/users.json";
        LoadDatabase();
        if (doneButton == null)
        {
            Debug.Log("Done Button is not assigned");
            return;
        }
    }
    private void Start() {
        doneButton.onClick.AddListener(AddUser);
    }
    private void OnDisable() {
        doneButton.onClick.RemoveListener(AddUser);
    }
    private void AddUser(){
        string username = newUserInput.GetUserName();
        if (string.IsNullOrWhiteSpace(username))
        {
            Debug.LogWarning("Username is empty");
            return;
        }
        if (database.users.Exists(u => u.username == username))
        {
            Debug.LogWarning("User already exists: " + username);
            return;
        }
        //Create new User
        User newUser = new User()
        {
            username = username,
            gamesPlayed = 0,
            highScore = 0
        };

        database.users.Add(newUser);
        SaveDatabase();

        Debug.Log("User created: " + username);
    }
    private void SaveDatabase() {
        string json = JsonUtility.ToJson(database, true);
        File.WriteAllText(filePath, json);
    }
    private void LoadDatabase() {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            database = JsonUtility.FromJson<UserDatabase>(json);
        }
        else
        {
            database = new UserDatabase();
            SaveDatabase();
        }
    }
}
