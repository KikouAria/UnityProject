using System.Collections;
using System.Collections.Generic;//访问附加的Mono类 同时还包括Dictionary类
using UnityEngine;

public class EventManager : MonoBehaviour
{
    // 事件管理器的实例（在这里主要是实现单例访问）
    private static EventManager instance = null;
    private float ClearTiming; //监听清理器计时器
    /*
     * 事件-监听器链表 键值对
     * 用来存储并管理所有的监听器
     */
    private Dictionary<Event.EVENT_TYPE, List<TListener>> Listeners = new Dictionary<Event.EVENT_TYPE, List<TListener>>();

    // 事件管理器的只读接口
    public static EventManager Instance
    {
        get { return instance; }
        set { }
    }

    // 在程序运行时调用 实现单例访问
    private void Awake()
    {
        ClearTiming = 0;
        //如果不存在事件管理器 则创建
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        //如果存在 则销毁自身
        else
            DestroyImmediate(this);

    }

    private void FixedUpdate()
    {
        ClearTiming += Time.deltaTime;
        //事件管理器每5秒执行一次无效监听器清理
        if (ClearTiming >= 5)
        {
            ClearTiming = 0;
            RemoveRedundancies();
        }
    }

    /// <summary>
    /// 注册监听器 在监听器类的Start函数调用
    /// </summary>
    /// <param name="eventType">监听的事件</param>
    /// <param name="listener">事件的监听器</param>
    public void AddListener(Event.EVENT_TYPE eventType, TListener listener)
    {
        List<TListener> ListenList = null;

        //如果当前事件的监听器不为空 则直接添加监听器至链表
        if (Listeners.TryGetValue(eventType, out ListenList))
        {
            for (int i = 0; i < ListenList.Count; i++)
            {
                if (!ListenList[i].Equals(null) && listener == ListenList[i])
                {
                    return;
                }
            }

            ListenList.Add(listener);
        }
        //如果当前事件的监听器为空 则新建链表 添加键值对
        else
        {
            ListenList = new List<TListener>();
            ListenList.Add(listener);
            Listeners.Add(eventType, ListenList);
        }
    }

    //删除事件
    public void RemoveListener(Event.EVENT_TYPE eventType, TListener listener)
    {
        if (listener == null)
        {
            return;
        }

        List<TListener> ListenList = null;

        //如果当前事件的监听器不为空 则直接添加监听器至链表
        if (Listeners.TryGetValue(eventType, out ListenList))
        {
            for (int i = 0; i < ListenList.Count; i++)
            {
                if (!ListenList[i].Equals(null) && listener == ListenList[i])
                {
                    ListenList[i] = null;
                    break;
                }
            }
        }
    }


    /// <summary>
    /// 事件传递函数 ===传递事件的关键函数===
    /// </summary>
    /// <param name="eventType">将要被处理的事件</param>
    /// <param name="sender">发送事件的组件</param>
    /// <param name="receiver">可选参数 指定响应本事件的对象</param>
    /// <param name="data">可选参数 可以传递参数</param>
    public bool PostNotification(Event.EVENT_TYPE eventType, Component sender, Object receiver = null, System.Object data = null)
    {
        List<TListener> ListenList = null;

        //如果事件对应的监听器为空 直接返回
        if (!Listeners.TryGetValue(eventType, out ListenList))
        {
            return false;
        }
        //遍历事件的所有监听器的事件处理函数
        for (int i = 0; i < ListenList.Count; i++)
        {
            //如果param不为空 则寻找特定的对象
            if (receiver)
            {
                if (!ListenList[i].Equals(null) && receiver == ListenList[i].GetGameObject())
                {
                    //寻找到特定对象后执行操作并返回操作是否成功
                    return ListenList[i].OnEvent(eventType, sender, receiver, data);
                }
            }

            //如果没有特定对象（广播事件）则遍历事件所有的监听器
            else if (!ListenList[i].Equals(null))
            {
                ListenList[i].OnEvent(eventType, sender, receiver, data);//调用函数 传参
            }

        }
        return true;
    }

    //注销事件
    public void RemoveEvent(Event.EVENT_TYPE eventType)
    {
        Listeners.Remove(eventType);
    }

    //删除无效的监听器
    private void RemoveRedundancies()
    {
        Dictionary<Event.EVENT_TYPE, List<TListener>> TmpListeners = new Dictionary<Event.EVENT_TYPE, List<TListener>>();

        //遍历所有事件
        foreach (KeyValuePair<Event.EVENT_TYPE, List<TListener>> Item in Listeners)
        {
            //遍历事件的所有监听器
            for (int i = Item.Value.Count - 1; i >= 0; i--)
            {
                //如果监听器的引用为null 则删除监听器
                if (Item.Value[i].Equals(null))
                    Item.Value.RemoveAt(i);
            }

            //重构监听器链表
            if (Item.Value.Count > 0)
                TmpListeners.Add(Item.Key, Item.Value);
        }

        Listeners = TmpListeners;
    }
}
