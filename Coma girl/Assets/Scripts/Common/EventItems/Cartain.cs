using System.Collections;
using System.Collections.Generic;
using Character;
using UI;
using UnityEngine;

namespace Common { 
    public class Cartain : EventItem
    {
        private bool _gotCartain;
        protected override void Init()
        {
            base.Init();
            id = 101;
            compoundTarget = 102;
            name = "窗帘";
        }

        public override void InteractEvent(Player player)
        {
            if (!_gotCartain){
                base.InteractEvent(player);
                JustAddToInventory();
                UIWindowManager.Instance.ShowMsg("你尝试撕下了一小块窗帘", "获得物品");
                _gotCartain = true;
            }
            else
            {
                UIWindowManager.Instance.ShowMsg("被撕了一角的破旧窗帘");
            }
        }
    }
}