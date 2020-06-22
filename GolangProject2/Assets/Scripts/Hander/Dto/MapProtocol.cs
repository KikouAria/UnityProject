using UnityEngine;
using System.Collections;

public class MapProtocol
{
    /// <summary>
    /// 申请进入地图
    /// </summary>
    public const int ENTER_CREQ = 1;
    /// <summary>
    /// 进入地图结果
    /// </summary>
    public const int ENTER_SRES = 2;
    /// <summary>
    /// 角色移动和旋转
    /// </summary>
    public const int MOVE_CREQ = 3;
    /// <summary>
    /// 移动和旋转结果
    /// </summary>
    public const int MOVE_SRES = 4;
    /// <summary>
    /// 申请退出地图
    /// </summary>
    public const int EXIT_CREQ = 5;
    /// <summary>
    /// 退出地图结果
    /// </summary>
    public const int EXIT_SRES = 6;
    /// <summary>
    /// 攻击请求
    /// </summary>
    public const int ATTACK_CREQ = 7;
    /// <summary>
    /// 攻击结果
    /// </summary>
    public const int ATTACK_SRES = 8;
    /// <summary>
    /// 通用协议
    /// </summary>
    public const int COMMON_MSG = 9;

}
