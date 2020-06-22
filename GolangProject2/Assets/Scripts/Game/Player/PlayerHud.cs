using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    public Text nameLabel;

    private int userId;
    public void SetUserId(int userId)
    {
        this.userId = userId;
    }

    // Update is called once per frame
    void Update()
    {
        PlayerData playerData = DataManager.Instance.GetPlayerData(this.userId);

        if (playerData != null)
        {
           Vector3 vDir = Camera.main.transform.position - transform.position;
            vDir.Normalize();
            transform.rotation = Quaternion.LookRotation(-vDir);

            nameLabel.text = playerData.name;
        
        }
    }

}