using UnityEngine;
using System.Collections;

public class SocketModel
{
    /// <summary>
    /// 模块类型
    /// </summary>
    public int type { get; set; }
    /// <summary>
    /// 命令协议
    /// </summary>
    public int command { get; set; }
    /// <summary>
    /// 消息体
    /// </summary>
    public string message { get; set; }
}
