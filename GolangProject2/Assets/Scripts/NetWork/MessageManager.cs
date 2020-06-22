using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MessageManager : MonoBehaviour {

    private LoginHandler login;
    private MapHandler map;

    void Start()
    {
        login = GetComponent<LoginHandler>();
        map = GetComponent<MapHandler>();
    }

    void Update()
    {
        while (true)
        {
            SocketModel model = NetWorkScript.Instance.GetListData();

            if (model != null)
            {
                
                OnMessage(model);
            }
            else
            {
                break;
            }
        }
    }

    public void OnMessage(SocketModel model)
    {
        // print("接收消息: "+ model.type + " " + model.command + " " + model.message);

        switch (model.type)
        {
            case Protocol.LOGIN:
                login.OnMessage(model);
                break;
            case Protocol.MAP:
                map.OnMessage(model);
                break;
            default:
                break;
        }
    }
}
