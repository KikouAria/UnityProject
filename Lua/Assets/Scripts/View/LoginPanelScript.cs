using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginPanelScript : MonoBehaviour
{
    public InputField account;

    public InputField password;

    // Start is called before the first frame update
    void Start()
    {
        account.text = "战神";
    }

    public void OnLogin()
    {
        Debug.Log("玩家名字为："+account.text);

        DataManager.Instance.SetPlayerName(account.text);
        gameObject.SetActive(false);

        GameController.Inst.GameEnter();
    }

}
