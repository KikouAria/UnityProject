using System;
using UnityEngine;
using Util;

namespace UI
{
    public class UITipsWindow: UIWindow
    {
        private UILabel _tipsLabel;

        public override void Init()
        {
            base.Init();
            animTime = 0.1f;
            var textObj = transform.FindAll("TipsText");
            if (!textObj)
                throw new Exception("Can't find Tips' Label Object");
            _tipsLabel = textObj.GetComponent<UILabel>();
            if (!_tipsLabel)
                _tipsLabel = textObj.gameObject.AddComponent<UILabel>();

        }


        public void ShowTips(string msg)
        {
            _tipsLabel.text = msg;
            SetVisible(true, false);

        }

        public void HideTips()
        {
            SetVisible(false, false);
        }




        private void OnGUI()
        {
            if (GUILayout.Button("测试测试121"))
            {
                ShowTips("窗帘");
            }           
            if (GUILayout.Button("测试测试122"))
            {
                HideTips();
            }
        }


    }
}