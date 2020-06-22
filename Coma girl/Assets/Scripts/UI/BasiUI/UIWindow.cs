using System;
using System.Collections;
using Assets.Scripts.Game;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// 所有UI窗口的父类, 所有UI窗口继承此类拥有基本窗口行为.
    /// </summary>
    public class UIWindow: MonoBehaviour
    {
        private UIPanel _panelComp;  // 用于设置窗口显隐
        private float _alpha = 0;  // 实际alpha值 用于保存窗口渐变显隐
        private UIWindowManager _uiWindowManager;


        /// <summary>
        /// 公开alpha值 改变此属性即可看到透明度渐变的动画效果
        /// </summary>
        public float Alpha
        {
            get => _alpha;
            set
            {
                if (Math.Abs(_alpha - value) >= 0.1)
                    StartCoroutine(AlphaChanger(value));
            }
        }

        private bool _visible;
        private bool Visible
        {
            get => _visible;
            set
            {
                if ((value && _alpha >0.99 || !value && _alpha < 0.01) && _visible == value)
                    return;

                _visible = value;
                Alpha = value ? 1 : 0;
                
            }
        }


        /// <summary>
        /// 显隐动画持续秒数
        /// </summary>
        public float animTime = 0.3f;

        void Start()
        {
            Init();
        }

        public virtual void Init()
        {
            // 为其添加CanvasGroup组件
            _panelComp = gameObject.GetComponent<UIPanel>();
            if (!_panelComp)
            {
                _panelComp = gameObject.AddComponent<UIPanel>();
            }

            _uiWindowManager = FindObjectOfType<UIWindowManager>();
            _panelComp.alpha = 0;



        }

        protected void SetVisible(bool visible, bool isLockCursor = true) // 设置窗口显隐 暴露接口
        {

            Visible = visible;
            if (isLockCursor)
            {
                GameManager.Instance.isLockCursor = !visible;
                _uiWindowManager.isWindowShowing = visible;
            }
            else
            {
                // _star.SetVisible(!visible);
            }
        }

        /// <summary>
        /// 窗口渐变显隐动画(协程)
        /// </summary>
        /// <param name="targetAlpha">设置的渐变透明度目标</param>
        /// <returns></returns>
        IEnumerator AlphaChanger(float targetAlpha)
        {
            var startTime = Time.time;
            float transValue = (targetAlpha - _alpha) / animTime;
            while (Math.Abs(targetAlpha - _alpha) > 0.05 && (Time.time-startTime <= animTime*1.1f))
            {
                _alpha += transValue * Time.deltaTime;
                _panelComp.alpha = _alpha;
                yield return new WaitForSeconds(Time.deltaTime);
            }

            _alpha = targetAlpha;
            _panelComp.alpha = targetAlpha;
            // this.gameObject.SetActive(false);
        }




    }




}
