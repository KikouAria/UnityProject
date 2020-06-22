using UnityEngine;
using System.Collections;

public class Networkadministrator : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        NetWorkScript.Instance.Init();
    }
}
