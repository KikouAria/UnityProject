using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Security.AccessControl;
using System.Security.Cryptography;
using Character;
using UI;
using UnityEngine;
namespace Common
{
    public abstract class EventItem: MonoBehaviour
    {
        /// <summary>
        /// 游戏物品的id
        /// </summary>
        public int id;
        /// <summary>
        /// 合成对象的id
        /// </summary>
        public int compoundTarget;
        /// <summary>
        /// 物品的显示名称
        /// </summary>
        [Tooltip("物体的显示名称")]
        public string name;

        /// <summary>
        /// 物品的图标 动态加载
        /// </summary>
        public Texture2D icon;
        /// <summary>
        /// 物品图标路径 用于动态加载图标
        /// </summary>
        public string icomPath;
        /// <summary>
        /// 互动次数 用于本地保存
        /// </summary>
        public int interactCount;

        /// <summary>
        /// 初始化物体 用于继承
        /// </summary>
        void Start()
        {
            Init();
        }
        protected virtual void Init()
        {

        }
        /// <summary>
        /// 和玩家互动时执行的函数 用于继承(由于需求简单 不使用委托)
        /// </summary>
        /// <param name="player"></param>
        public virtual void InteractEvent(Player player) 
        {

        }
        /// <summary>
        /// 玩家选择"使用" 该道具时执行单函数 用于继承(由于需求简单 不使用委托)
        /// </summary>
        /// <param name="eventItem"></param>
        public virtual void Using(EventItem eventItem)
        {

        }
        /// <summary>
        /// 玩家选择 "合成" 时 执行的函数 (同上)
        /// </summary>
        /// <param name="targetId"></param>
        public virtual void Compound(int targetId)
        {

        }
        /// <summary>
        /// 工具函数 将该物体加入到玩家道具栏
        /// </summary>
        /// <param name="player"></param>
        public void AddToInventory(Player player)
        {
            UIWindowManager.Instance.ShowMsg("你获得了一个" + name + "!", "获得物品");
            InventoryManager.Instance.AddItem(this);
            transform.localScale = Vector3.one * 0.001f;
            
        }

        public void JustAddToInventory()
        {
            InventoryManager.Instance.AddItem(this);
        }
    }
}