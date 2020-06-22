using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Common;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 道具栏系统 单例类 继承UIWindow实现显隐和单例 若场景无则会自动创建 
    /// </summary>
    public class InventoryManager:MonoSingleton<InventoryManager>
    {

        private List<EventItem> _itemList;
        
        
        public override void Init()
        {
            _itemList = new List<EventItem>();
            
        }



        /// <summary>
        /// 添加一个道具到道具栏
        /// </summary>
        /// <param name="item">添加的道具</param>
        public void AddItem(EventItem item)
        {
            // 动态加载所需物体的图标
            Texture2D itemTexture = Resources.Load<Texture2D>("Texture/" + item.icomPath);
            item.icon = itemTexture;
            // 添加到道具表
            _itemList.Add(item);
        }

        /// <summary>
        /// 根据id删除背包中的相应道具
        /// </summary>
        /// <param name="itemId"></param>
        public bool RemoveItem(int itemId)
        {
            foreach (var eachItem in _itemList)
            {
                if (eachItem.id == itemId)
                    _itemList.Remove(eachItem);
                return true;
            }

            return false;

        }
        /// <summary>
        /// 打开/关闭背包
        /// </summary>



        private void OnGUI()
        {
            if (GUILayout.Button("测试ToggleInventory"))
            {
               
            }
        }


    }
}