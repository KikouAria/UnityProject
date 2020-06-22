using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Game;
using Common;
using UI;
using UnityEngine;

namespace Character
{
    public class Player : MonoBehaviour
    {

        public EventItem interactableItem;
        public EventItem handItem;
        public List<EventItem> inventory;
        public Camera characterCamera;
        void Start()
        {
            if (!characterCamera)
                characterCamera = FindObjectOfType<Camera>();
            if (!characterCamera)
                throw new Exception("CantFindAnyCamera!");
            GameManager.Instance.isLockCursor = true;
        }

        void Update()
        {
            UpdateInteractableItem();
        }

        void UpdateInteractableItem()
        {
            RaycastHit hitInfo;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hitInfo, 3, 1<<9))
            {
                var item = hitInfo.transform;
                var eventItem = hitInfo.transform.gameObject.GetComponent<EventItem>();
                // print(item);
                // print(eventItem.name);
                interactableItem = eventItem;
                if (eventItem && !string.IsNullOrEmpty(eventItem.name))
                    UIWindowManager.Instance.ShowTips(eventItem.name);

            }
            else
            {
                // print("nothing to interact");
                interactableItem = null;
                UIWindowManager.Instance.HideTips();
                
            }

        }

        void ChangeStatus(PlayerStatus status)
        {

        }

        public void DoInteraction()
        {
            // print("尝试执行交互");
            interactableItem?.InteractEvent(this);

        }
    }
}