using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject enemy;
    public float bornTime = 10f;
    public Transform bornPos;
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Born", bornTime, bornTime);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Born()
    {
        Instantiate(enemy, bornPos.position, bornPos.rotation);
    }
}

