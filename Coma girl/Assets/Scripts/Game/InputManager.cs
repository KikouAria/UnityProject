using System;
using Character;
using UI;
using UnityEngine;
using Util;

namespace Assets.Scripts.Game
{
    /// <summary>
    /// 专职进行Input的接收 判断当前状态 执行按钮操作
    /// </summary>
    public class InputManager: MonoSingleton<InputManager>
    {
        private Player _player;
        private UIWindowManager _uiWindowManager;
        void Start()
        {
            _player = FindObjectOfType<Player>();
            if (!_player)
                throw new Exception("Cant Find Player!");
            _uiWindowManager = FindObjectOfType<UIWindowManager>();
            if (!_uiWindowManager)
                throw new Exception("Cant Find Player!");
        }

        void Update()
        {
            // 如果是在PC平台
#if UNITY_STANDALONE_WIN
            if (Input.GetMouseButton(0))
            {
                if (!_uiWindowManager.isWindowShowing)
                    _player.DoInteraction();
            }

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                GameManager.Instance.isLockCursor = !GameManager.Instance.isLockCursor;
            }
#elif UNITY_ANDROID
            // 如果是Android 则执行这里的输入控制
            
#elif UNITY_IPHONE
            // 如果是苹果 则是这里的输入控制
#endif
        }
    }
}