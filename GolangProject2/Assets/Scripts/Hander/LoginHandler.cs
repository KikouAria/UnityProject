using UnityEngine;
using System.Collections;


public class LoginHandler : MonoBehaviour
{

    enum LOGIN_RESULT
    {
        LOGIN_RET_SUCC = 0,	// 登陆成功
        LOGIN_RET_NOT, 	    // 登陆失败 用户信息不存在
        LOGIN_RET_PWD,	    // 登陆失败 用户密码错误
    };

    void Start()
    {

    }

    void Update()
    {
        
    }

    public void OnMessage(SocketModel model)
    {
        switch (model.command)
        {
            case LoginProtocol.REG_SRES:
                RegResult(model.message);
                break;
            case LoginProtocol.LOGIN_SRES:
                LoginResult(model.message);
                break;
        }
    }

    private void RegResult(string message)
    {
        RegSResDto dto = Coding<RegSResDto>.decode(message);
        if (dto.result)
        {
            Debug.Log(dto.name+", 注册玩家信息成功");
            return;
        }
        Debug.Log(dto.name+", 注册玩家信息失败");
    }

    private void LoginResult(string message)
    {
        LoginSResDto dto = Coding<LoginSResDto>.decode(message);


        if (dto.result != (int)LOGIN_RESULT.LOGIN_RET_SUCC)
        {
            Debug.Log(dto.name + ", 玩家登陆失败");

            string text = GetResultString(dto.result);
            text = dto.name + ":" + text;

            MessageBoxUI.Show(text);
            return;
        }

        Debug.Log(dto.name + ", 玩家登陆成功");
        if (dto.userid != -1)
            DataManager.Instance.SetGamePlayer(dto.userid);
        DataManager.Instance.AddPlayer(dto.name, dto.userid);

        SendMsgEnterMap(dto.userid);

        EventManager.Instance.PostNotification(Event.EVENT_TYPE.LOGIN_GAME_SUCC, this);
    }

    private void SendMsgEnterMap(int userid)
    {
        //发消息申请进入场景
        EnterCReqDto dto = new EnterCReqDto();
        dto.userid = userid;

        string message = Coding<EnterCReqDto>.encode(dto);
        NetWorkScript.Instance.SendMessage(Protocol.MAP, MapProtocol.ENTER_CREQ, message);
    }

    private string GetResultString(int result)
    {
        switch ((LOGIN_RESULT)result)
        {
            case LOGIN_RESULT.LOGIN_RET_SUCC:
                return "登陆成功！";
            case LOGIN_RESULT.LOGIN_RET_NOT:
                return "登陆失败 用户信息不存在！";
            case LOGIN_RESULT.LOGIN_RET_PWD:
                return "登陆失败 用户密码错误！";

        }

        return "";
    }


}
