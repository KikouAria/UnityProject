using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class PlayerManager : MonoBehaviour, TListener
{
    public GameObject player = null;                // The player prefab to be spawned.

    private Dictionary<int, GameObject> dicPlayes = new Dictionary<int, GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //注册监听器
        EventManager.Instance.AddListener(Event.EVENT_TYPE.ENTER_MAP_SUCC, this);
        EventManager.Instance.AddListener(Event.EVENT_TYPE.EXIT_MAP_SUCC, this);

    }

    /// <summary>
    /// 事件处理接口
    /// </summary>
    /// <param name="eventType">事件类型</param>
    /// <param name="sender">发送事件的游戏组件</param>
    /// <param name="receiver">可选参数 接收事件的游戏组件</param>
    /// <param name="value">可选参数 可传递游戏数值</param>
    public bool OnEvent(Event.EVENT_TYPE eventType, Component sender, Object receiver = null, System.Object data = null)
    {
        // 进入场景成功
        if (eventType == Event.EVENT_TYPE.ENTER_MAP_SUCC)
        {

            EnterSResDto dto = data as EnterSResDto;

            // Create an instance of the player prefab at the randomly selected spawn point's position and rotation.
            Vector3 pos = new Vector3((float)dto.point[0], (float)dto.point[1], (float)dto.point[2]);
            Quaternion rot = Quaternion.Euler((float)dto.rotation[0], (float)dto.rotation[1], (float)dto.rotation[2]);

            // 创建! 角色对象!!
            GameObject gamePlayer = Instantiate(player, pos, rot);
            if (dto.userid == -1)
            {
                gamePlayer.tag = "Monster";

            }

            gamePlayer.name = dto.name;
            int userId = dto.userid;
            dicPlayes.Add(userId, gamePlayer);
            
            PlayerHud playerHud = gamePlayer.GetComponentInChildren<PlayerHud>();
            playerHud.SetUserId(userId);

            PlayerUpdater updater = gamePlayer.GetComponent<PlayerUpdater>();
            updater.SetUserId(userId);

            PlayerShooting playerShooting = gamePlayer.GetComponentInChildren<PlayerShooting>();
            playerShooting.SetUserId(userId);

            if (DataManager.Instance.IsGamePlayer(userId))
            {
                gamePlayer.GetComponent<PlayerMovement>().enabled = true;

                CameraFollow script = Camera.main.GetComponent<CameraFollow>();
                script.SetTarget(gamePlayer.GetComponent<Transform>());
                script.enabled = true;
            }
            else
            {
                updater.isLocalPlayer = false;
                playerShooting.isLocalPlayer = false;
            }

            return true;

        }
        // 退出场景成功 销毁对象
        else if (eventType == Event.EVENT_TYPE.EXIT_MAP_SUCC)
        {
            ExitSResDto dto = data as ExitSResDto;
            int userId = dto.userid;

            GameObject gameplayer = null;

            //如果当前事件的监听器不为空 则直接添加监听器至链表
            if (dicPlayes.TryGetValue(userId, out gameplayer))
            {
                if (DataManager.Instance.IsGamePlayer(userId))
                {
                    CameraFollow script = Camera.main.GetComponent<CameraFollow>();
                    script.enabled = false;
                }

                dicPlayes.Remove(userId);
                Destroy(gameplayer);
            }

            return true;
        }

        return false;
    }

    /// <summary>
    /// 获取当前游戏对象的引用
    /// </summary>
    /// <returns>当前类游戏对象的引用（只读）</returns>
    public Object GetGameObject()
    {
        return this;
    }

       
}