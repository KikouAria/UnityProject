using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RegPanelScript : MonoBehaviour
{
    public InputField account;

    public InputField password;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    public void OnConfirm()
    {
        string strAcc = account.text.Trim();
        string strPwd = password.text.Trim();

        if (string.IsNullOrEmpty(strAcc) || string.IsNullOrEmpty(strPwd))
        {
            Debug.Log("空账户");
            // Debug.Log("用户名或则密码错误！！！请检查");
            // return;
        }

        Debug.Log("申请注册账号");

        RegCReqDto dto = new RegCReqDto();
        dto.username = strAcc;
        dto.password = strPwd;
        string message = Coding<RegCReqDto>.encode(dto);
        NetWorkScript.Instance.SendMessage(Protocol.LOGIN, LoginProtocol.REG_CREQ, message);
        gameObject.SetActive(false);
    }

    public void OnCancel()
    {
        gameObject.SetActive(false);
    }
}
