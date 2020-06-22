using System;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using Util;

namespace UI
{
    /// <summary>
    /// 向外暴露窗口显隐相关的所有接口
    /// </summary>
    public class UIWindowManager: MonoSingleton<UIWindowManager>
    {
        private UIMessageWindow _messageWindowComp;
        private UITipsWindow _tipsWindow;
        public bool isWindowShowing;
        void Start()
        {
            _messageWindowComp = GameObject.FindObjectOfType<UIMessageWindow>();
            if (!_messageWindowComp)
                throw new Exception("Cant find MessageWindow!");
            _tipsWindow = GameObject.FindObjectOfType<UITipsWindow>();

        }

        

        /// <summary>
        /// 显示一个信息对话框
        /// </summary>
        public void ShowMsg(string msg, string title = "信息")
        {
            _messageWindowComp.ShowMsg(msg, title);
        }

        /// <summary>
        /// 显示一连串的连续的对话窗口
        /// </summary>
        /// <param name="dialogList"></param>
        public void ShowDiaLog(List<string> dialogList)
        {

        }

        /// <summary>
        /// 在屏幕中心显示小小的一串字符
        /// </summary>
        /// <param name="msg"></param>
        public void ShowTips(string msg)
        {
            _tipsWindow.ShowTips(msg);
        }

        public void HideTips()
        {
            _tipsWindow.HideTips();
        }

    }
}