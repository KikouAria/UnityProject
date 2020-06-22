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
        
    }

    private string playerName;

    public void SetPlayerName(string name)
    {
        playerName = name;
    }

    public string GetPlayerName()
    {
        return playerName;
    }

}
