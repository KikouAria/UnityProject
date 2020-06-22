using Character;
using UI;
using UnityEngine;

namespace Common
{
    /// <summary>
    /// 普通的物件 任何点击后仅仅是显示对话框的物体都能加此脚本
    /// 需要碰撞盒(用于射线检测), 并且要将Layer设置为9: EventItem
    /// </summary>
    public class NormalItem: EventItem
    {
        [Tooltip("在此填写对话框的内容")]
        public string description = "";
        protected override void Init()
        {            
            base.Init();
            id = -1;
            if (string.IsNullOrEmpty(name))
                name = "物体";
            if (string.IsNullOrEmpty(description))
                description = "似乎只是个普通的" + name;

        }


        public override void InteractEvent(Player player)
        {
            base.InteractEvent(player);
            UIWindowManager.Instance.ShowMsg(description);
        }
    }
}
