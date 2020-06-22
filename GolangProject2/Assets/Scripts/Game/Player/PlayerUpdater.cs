using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class PlayerUpdater : MonoBehaviour, TListener
{
    private Vector3 syncPos; //同步变量
    private Transform myTransform;

    private float lerpRate;
    //private float normalLerpRate = 16.0f;
    //private float fasterLerpRate = 27.0f;
    private float normalLerpRate = 160.0f;
    private float fasterLerpRate = 270.0f;

    private Vector3 lastPos = new Vector3();
    private float threshold = 0.01f;

    private List<Vector3> syncPosList = new List<Vector3>();
    private float closeEnough = 0.1f;

    public bool useHistoriicalLerping = true; //是否启用平滑插值的开关，直接在 inspector 中设置
    public bool isLocalPlayer = false;

    private int userId;
    public void SetUserId(int userId)
    {
        this.userId = userId;
    }

    void Start()
    {
        lerpRate = normalLerpRate;
        myTransform = this.transform;
        lastPos = new Vector3(myTransform.position.x, myTransform.position.y, myTransform.position.z);

        //注册监听器
        EventManager.Instance.AddListener(Event.EVENT_TYPE.MOVE_MAP_SUCC, this);
        EventManager.Instance.AddListener(Event.EVENT_TYPE.ATTACK_MAP_SUCC, this);

        this.InvokeRepeating("OnInterval", 0.05f, 0.05f);
    }

    void OnDestory()
    {
        //删除监听器
        EventManager.Instance.RemoveListener(Event.EVENT_TYPE.MOVE_MAP_SUCC, this);
        EventManager.Instance.RemoveListener(Event.EVENT_TYPE.ATTACK_MAP_SUCC, this);
    }

    void Update()
    {
        LerpPosition(); //因为方法利用了Time.deltaTime，所以只能在 Updata中调用
    }

    void OnInterval() //1. server 和 client OnInterval
    {
        TransmitPosition(); //2. 因为是 ClientCallback，所以只有客户端调用
    }

    void TransmitPosition()
    {
        if (userId == -1) return;
        // if (isLocalPlayer && Vector3.Distance(myTransform.position, lastPos) > threshold) //3. 只用本机玩家才提交位置信息到server上
        if (isLocalPlayer) // 一定上传
        {
            CmdProvidePositionToServer(myTransform.position);
            // print("上传当前坐标: id: " + userId);
        }
    }

    void CmdProvidePositionToServer(Vector3 pos)
    {
        lastPos = pos; //4. 发送服务端信息同步给所有客户端的该对象的syncPos变量

        MoveCReqDto dto = new MoveCReqDto();

        dto.point[0] = myTransform.position.x;
        dto.point[1] = myTransform.position.y;
        dto.point[2] = myTransform.position.z;

        dto.rotation[0] = myTransform.eulerAngles.x;
        dto.rotation[1] = myTransform.eulerAngles.y;
        dto.rotation[2] = myTransform.eulerAngles.z;

        string message = Coding<MoveCReqDto>.encode(dto);
        NetWorkScript.Instance.SendMessage(Protocol.MAP, MapProtocol.MOVE_CREQ, message);

    }

    void LerpPosition()
    {
        if (!isLocalPlayer) //5. 只有非本机玩家才进行插值移动到最新的 syncPos 位置
        {
            if (useHistoriicalLerping) //更加平滑
            {
                HistoryLerping();
            }
            else
            {
                OrdinaryLerping();
            }
        }
    }

    public void SyncPostionsValues(Vector3 lastPos)
    {
        syncPos = lastPos;
        syncPosList.Add(syncPos); //将所有服务端同步过来的 pos 全都保存在队列中
    }
    
    void OrdinaryLerping() //普通插值，有卡顿现象
    {
        myTransform.position = Vector3.Lerp(myTransform.position, syncPos, Time.deltaTime * lerpRate);
    }

    void HistoryLerping() //平滑插值
    {
        if (syncPosList.Count > 0)
        {
            //取出队列中的第一个设为插值的目标
            myTransform.position = Vector3.Lerp(myTransform.position, syncPosList[0], Time.deltaTime * lerpRate);
            //位置足够接近，从队列中移除第一个，紧接着就是第二个
            if (Vector3.Distance(myTransform.position, syncPosList[0]) < closeEnough)
            {
                syncPosList.RemoveAt(0);
            }
            //如果同步队列过大，加快插值速率，使其更快到达目标点
            if (syncPosList.Count > 3)
            {
                lerpRate = fasterLerpRate;
            }
            else
            {
                lerpRate = normalLerpRate;
            }
            //Debug.LogFormat("--- syncPosList, count:｛0｝", syncPosList.Count);
        }
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
        if (eventType == Event.EVENT_TYPE.MOVE_MAP_SUCC)
        {
            MoveSResDto dto = data as MoveSResDto;

            // print("userId: " + userId + "dtoId" + dto.userid);

            if (this.userId != dto.userid)
            {
                return false;
            }

            Vector3 pos = new Vector3((float)dto.point[0], (float)dto.point[1], (float)dto.point[2]);
            Quaternion rot = Quaternion.Euler((float)dto.rotation[0], 0, (float)dto.rotation[2]);

            SyncPostionsValues(pos);
            transform.LookAt(new Vector3((float)dto.rotation[0], (float)dto.rotation[1], (float)dto.rotation[2]) + pos);

            return true;
        }
        else if(eventType == Event.EVENT_TYPE.ATTACK_MAP_SUCC)
        {
            AttackSResDto dto = data as AttackSResDto;

            if (this.userId != dto.useridatk)
            {
                return false;
            }

            Vector3 point = new Vector3((float)dto.pointDest[0], (float)dto.pointDest[1], (float)dto.pointDest[2]);
            Vector3 dir = point - myTransform.position;

            myTransform.rotation = Quaternion.LookRotation(dir);
        }

        return false;
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
