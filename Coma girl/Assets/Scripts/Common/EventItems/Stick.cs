using System.Collections;
using System.Collections.Generic;
using Character;
using UI;
using UnityEngine;

namespace Common { 
    public class Stick : EventItem
    {

        protected override void Init()
        { 
            base.Init(); 
            id = 102; 
            compoundTarget = 101; 
            name = "木棍";
         }

        public override void InteractEvent(Player player)
        {
            base.InteractEvent(player);
            AddToInventory(player);
            transform.localScale = Vector3.one * 0.001f;
        }

        public override void Compound(int targetId)
        {
            base.Compound(targetId);
            if (targetId == 101)
            {
                InventoryManager.Instance.RemoveItem(101);
                InventoryManager.Instance.AddItem(new Torch());
            }
        }
    }
}