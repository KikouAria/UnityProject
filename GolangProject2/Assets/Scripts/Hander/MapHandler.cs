using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class MapHandler : MonoBehaviour
{
    void Start()
    {

    }

    void Update()
    {

    }

    public void OnMessage(SocketModel model)
    {
        switch (model.command)
        {
            case MapProtocol.ENTER_SRES:
                MapEnter(model.message);
                break;
            case MapProtocol.MOVE_SRES:
                MapMove(model.message);
                break;
            case MapProtocol.EXIT_SRES:
                MapExit(model.message);
                break;
            case MapProtocol.ATTACK_SRES:
                MapAttack(model.message);
                break;
            case MapProtocol.COMMON_MSG:
                MapCommon(model.message);
                break;

        }
    }

    private void MapEnter(string message)
    {
        EnterSResDto dto = Coding<EnterSResDto>.decode(message);
        if (dto.name == "")
        {
            Debug.Log(dto.name + ", 空账户进入地图");
            // Debug.Log(dto.name + ", 进入地图失败");
            // return;
        }

        Debug.Log(dto.name + ", 进入地图成功");

        // 创建角色
        DataManager.Instance.AddPlayer(dto.name, dto.userid);

        EventManager.Instance.PostNotification(Event.EVENT_TYPE.ENTER_MAP_SUCC, this, null, dto);

        // 测试通用协议
        //TestCommonMsg();
    }

    private void MapMove(string message)
    {
        MoveSResDto dto = Coding<MoveSResDto>.decode(message);
        if (dto.userid == 0)
        {
            Debug.Log(dto.userid + ", 在地图移动和旋转失败");
            return;
        }

        //Debug.Log(dto.userid + ", 在地图移动和旋转成功");

        EventManager.Instance.PostNotification(Event.EVENT_TYPE.MOVE_MAP_SUCC, this, null, dto);
    }

    private void MapExit(string message)
    {
        ExitSResDto dto = Coding<ExitSResDto>.decode(message);
        if (dto.userid == 0)
        {
            Debug.Log(dto.userid + ", 退出地图失败");
            return;
        }

        Debug.Log(dto.userid + ", 退出地图成功");

        // 删除角色
        DataManager.Instance.RemovePlayerData(dto.userid);

        EventManager.Instance.PostNotification(Event.EVENT_TYPE.EXIT_MAP_SUCC, this, null, dto);

    }

    private void MapAttack(string message)
    {
        AttackSResDto dto = Coding<AttackSResDto>.decode(message);
        if (dto.useridatk == 0)
        {
            Debug.Log(dto.useridatk + ", 攻击失败");
            return;
        }

        //Debug.Log(dto.useridatk + ", 攻击");
        EventManager.Instance.PostNotification(Event.EVENT_TYPE.ATTACK_MAP_SUCC, this, null, dto);
        EventManager.Instance.PostNotification(Event.EVENT_TYPE.PLAYER_ATTACK, this, null, dto);

    }

    private void TestCommonMsg()
    {
        Debug.Log("-------测试通用协议发送");
        AttackCReqDto dto = new AttackCReqDto();
        dto.useridhit = 101;
        dto.pointSrc[0] = 2;
        dto.pointSrc[1] = 3;
        dto.pointSrc[2] = 4;

        dto.pointDest[0] = 0.1;
        dto.pointDest[1] = 0.2;
        dto.pointDest[2] = 0.3;

        string message = Coding<AttackCReqDto>.encode(dto);
        NetWorkScript.Instance.SendMessage(Protocol.MAP, MapProtocol.COMMON_MSG, message);
    }

    private void MapCommon(string message)
    {
        //测试通用协议
        AttackCReqDto dto = Coding<AttackCReqDto>.decode(message);
        Debug.Log("-------测试通用协议接收");
        Debug.Log(dto.useridhit);
        Debug.Log(dto.pointSrc[0]);
        Debug.Log(dto.pointSrc[1]);
        Debug.Log(dto.pointSrc[2]);
        Debug.Log(dto.pointDest[0]);
        Debug.Log(dto.pointDest[1]);
        Debug.Log(dto.pointDest[2]);
    }
}
