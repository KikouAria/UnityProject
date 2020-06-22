using UnityEngine;
using Util;

namespace UI
{
    public class UIMessageWindow: UIWindow
    {
        public override void Init()
        {
            base.Init();

        }

        public void ShowMsg(string msg, string title = "信息")
        {
            Cursor.lockState = CursorLockMode.None;
            SetVisible(true);
            transform.FindAll("TitleText").GetComponent<UILabel>().text = title;
            transform.FindAll("ContentText").GetComponent<UILabel>().text = msg;

        }

        public void CloseWindow()
        {
            SetVisible(false);
        }

        private void OnGUI()
        {
            if (GUILayout.Button("测试测试111"))
            {
                ShowMsg("Wow??");
            }
        }
    }






}