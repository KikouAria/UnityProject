using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DataManager
{
    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();

            }
            return instance;
        }
    }

    public DataManager()
    {
        allPlayerData = new Dictionary<int, PlayerData>();
        gamePlayfer = 0;
    }

    private Dictionary<int, PlayerData> allPlayerData;
    private int gamePlayfer;

    public void SetGamePlayer(int userId)
    {
        gamePlayfer = userId;
    }

    public PlayerData GetGamePlayer()
    {
        return GetPlayerData(gamePlayfer);
    }

    public bool IsGamePlayer(int userId)
    {
        if (userId != 0 && userId == gamePlayfer)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddPlayer(string userName, int userId, bool isMonster = false)
    {
        allPlayerData.Remove(userId);

        PlayerData data = new PlayerData {name = userName, userid = userId, isMonster = isMonster};
        allPlayerData.Add(userId, data);

    }

    public PlayerData GetPlayerData(int userId)
    {
        PlayerData data;
        allPlayerData.TryGetValue(userId, out data);
        return data;
    }

    public void RemovePlayerData(int userId)
    {
        allPlayerData.Remove(userId);
    }
}
