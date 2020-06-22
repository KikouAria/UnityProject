using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private new Transform transform;
    private Transform player;
    private Rigidbody rigid;
    private NavMeshAgent agent;
    
    private float moveSpeed = 6f;
 
    // Start is called before the first frame update
    private void Awake()
    {   
        transform = GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rigid = this.GetComponent<Rigidbody>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.speed = moveSpeed;
       
    }
    public void SetTarget(Transform player)
    {
        this.target = player;
        
        transform.position = new Vector3(transform.position.x + player.position.x, transform.position.y, transform.position.z + player.position.z);
    }

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        agent.SetDestination(player.position);
    }
    private void FixedUpdate()
    {
        Vector3 direction = player.position - transform.position;
        Vector3 deltaVelocity = direction - rigid.velocity;
        rigid.AddForce(deltaVelocity, ForceMode.Force);
    }
}
