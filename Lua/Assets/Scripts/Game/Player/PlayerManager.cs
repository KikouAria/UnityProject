using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;    //单例

    public GameObject player = null;                // The player prefab to be spawned.

    static private int _uid = 0;

    public static int createUid()
    {
        return ++_uid;
    }
    
    void Awake()
    {
        _instance = this;
    }

    public static GameObject GetGameObject()
    {
        return _instance.player;
    }

}