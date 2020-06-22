using System.Collections;
using System.Collections.Generic;
using Character;
using UI;
using UnityEngine;

namespace Common { 
    public class Stove : EventItem
    {
        protected override void Init()
        {
            base.Init();
            id = 104;
            compoundTarget = 0;
            name = "壁炉";
        }


        public override void InteractEvent(Player player)
        {
            base.InteractEvent(player);
            if (player.handItem && player.handItem.id == 103)
            {
                UIWindowManager.Instance.ShowMsg("火把被点燃了!");
                InventoryManager.Instance.AddItem(new FiredTorch());
                InventoryManager.Instance.RemoveItem(102);
            }
            else
            {
                UIWindowManager.Instance.ShowMsg("一个普通的壁炉 里面燃烧着熊熊烈火");
            }
        }
    }
}