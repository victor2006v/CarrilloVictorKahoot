using System.Collections.Generic;
using System;

[Serializable]
public class UserData
{
    public string userName;
    public int points = 0;
}

[Serializable]
public class UserList
{
    public List<UserData> users = new List<UserData>();
}
