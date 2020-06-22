using LuaInterface;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {


    private static GameController _instance;

    private LuaTable gamePlay = null;

    private float timer = 1.0f;

    public bool isPlaying = false;
    public LuaState state;
    public static GameController Inst
    {
        get
        {
            return _instance;
        }
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        //创建lua虚拟机
        state = new LuaState();
        state.Start();

        string sceneFile = Application.dataPath + "/LuaScripts";
        state.AddSearchPath(sceneFile);

        //绑定lua虚拟机，注册方法调用
        LuaBinder.Bind(state);

        gamePlay = state.DoFile<LuaTable>("gameplay.lua");
        //gamePlay.Dispose();
    }


    public void GameEnter()
    {
        if (gamePlay != null)
        {
            gamePlay.Call("GameStart", gamePlay);

            isPlaying = true;
        }
    }

    public void onShootHit(int uid)
    {
        if (gamePlay != null)
        {
            gamePlay.Call("onShootHit", gamePlay, uid);
        }
    }

    void Update()
    {
        if (isPlaying)
        {
            onTime();
        }
    }

    public void onTime()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = 0.1f;

            if(gamePlay != null)
            {
                // print(gamePlay);
                gamePlay.Call("GameTimer", gamePlay);
            }
        }
    }

}
