using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface TListener
{
    /// <summary>
    /// 事件处理接口
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="sender">发送事件的游戏组件</param>
    /// <param name="receiver">可选参数 接收事件的游戏组件</param>
    /// <param name="value">可选参数 可传递游戏数值</param>
    bool OnEvent(Event.EVENT_TYPE eventType, Component sender, Object receiver = null, System.Object data = null);

    /// <summary>
    /// 获取当前游戏对象的引用
    /// </summary>
    /// <returns>当前类游戏对象的引用（只读）</returns>
    Object GetGameObject();
}
