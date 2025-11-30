using System.Collections.Generic;

[System.Serializable]
public class User
{
    public string username;
    public int gamesPlayed;
    public int highScore;
}
[System.Serializable]
public class UserDatabase {
    public List<User> users = new List<User>();
}
