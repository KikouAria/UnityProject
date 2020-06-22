using UI;
using UnityEngine;
using Util;

namespace Assets.Scripts.Game
{
    public class GameManager:MonoSingleton<GameManager>
    {
        public bool isLockCursor = true ;


        void Start()
        {

        }
        void Update()
        {
            UpdateLockCursor();
        }

        void UpdateLockCursor()
        {


            Cursor.lockState = isLockCursor ? CursorLockMode.Locked : CursorLockMode.None;

        }
    }
}