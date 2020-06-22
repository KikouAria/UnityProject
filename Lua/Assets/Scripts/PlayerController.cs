using UnityEngine;
using System.Collections;
using LuaInterface;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed;
    private Text countText;
    private Text winText;
    private LuaState state;

    private int count;
    private LuaTable pickCubeUp;  // Lua脚本
    private void Awake()
    {
        countText = GameObject.Find("Canvas").transform.Find("Count Text").GetComponent<Text>();
        winText = GameObject.Find("Canvas").transform.Find("Win Text").GetComponent<Text>();
        
    }
    void Start()
    {
        count = 0;
        SetCountText();

        // 获取GameController上的lua虚拟机
        state = GameController.Inst.state;


    }
    void FixedUpdate()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        //Debug.Log("moveHorizontal="+ moveHorizontal);
        //Debug.Log("moveVertical=" + moveVertical);

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        this.GetComponent<Rigidbody>().AddForce(movement * speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PickUp")
        {

            other.gameObject.SetActive(false);
            count++;
            SetCountText();

        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "PickUp")
        {
            /*
            Destroy(collision.collider.gameObject);
            count = count + 1;
            SetCountText();
            */

            Debug.Log("OnCollisionEnter");
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.collider.tag == "PickUp")
        {
            Debug.Log("OnCollisionStay");
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.tag == "PickUp")
        {
            Debug.Log("OnCollisionExit");
        }
    }


    void SetCountText()
    {
        countText.text = "Score: " + count.ToString();

    }


}
