using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelScript : MonoBehaviour, TListener
{
    public InputField account;

    public InputField password;

    public GameObject regPanel; 

    // Start is called before the first frame update
    void Start()
    {
        //注册监听器
        EventManager.Instance.AddListener(Event.EVENT_TYPE.LOGIN_GAME_SUCC, this);

    }

    // Update is called once per frame
    public void OnRegister()
    {
        if (regPanel != null)
        {
            regPanel.SetActive(true);
        }
    }

    public void OnLogin()
    {
        string strAcc = account.text.Trim();
        string strPwd = password.text.Trim();

        if (string.IsNullOrEmpty(strAcc) || string.IsNullOrEmpty(strPwd))
        {
            Debug.Log("空账户");
            // Debug.Log("用户名或则密码错误！！！请检查");
            // return;
        } 

        Debug.Log("申请登陆");

        LoginCReqDto dto = new LoginCReqDto();

        dto.username = strAcc;
        dto.password = strPwd;
        string message = Coding<LoginCReqDto>.encode(dto);
        NetWorkScript.Instance.SendMessage(Protocol.LOGIN, LoginProtocol.LOGIN_CREQ, message);

    }

    /// <summary>
    /// 事件处理接口
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="sender">发送事件的游戏组件</param>
    /// <param name="receiver">可选参数 接收事件的游戏组件</param>
    /// <param name="value">可选参数 可传递游戏数值</param>
    public bool OnEvent(Event.EVENT_TYPE eventType, Component sender, Object receiver = null, System.Object data = null)
    {
        if (eventType == Event.EVENT_TYPE.LOGIN_GAME_SUCC) {
            gameObject.SetActive(false);
        }

        return true;
    }

    /// <summary>
    /// 获取当前游戏对象的引用
    /// </summary>
    /// <returns>当前类游戏对象的引用（只读）</returns>
    public Object GetGameObject()
    {
        return this;
    }
}
