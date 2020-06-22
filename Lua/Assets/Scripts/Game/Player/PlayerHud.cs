using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHud : MonoBehaviour
{
    public Text nameLabel;

    private string playerName;

    public void SetName(string name)
    {
        this.playerName = name;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.playerName != null)
        {
           Vector3 vDir = Camera.main.transform.position - transform.position;
            vDir.Normalize();
            transform.rotation = Quaternion.LookRotation(-vDir);

            nameLabel.text = this.playerName;
        }
    }

}