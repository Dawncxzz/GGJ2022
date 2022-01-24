using System;
using Framework;
using UnityEngine;
using UnityEngine.UI;

namespace Logic
{
    public class PlayerInput
    {
        public InputButton jumpBtn;
        public InputButton slideBtn;
        public InputButton diveBtn;
        public InputButton hurtBtn;//测试用

        public PlayerInput()
        {
            Load();
        }
        
        public void Load()
        {
            IInputModule inputModule = SkyFrameworkEntry.GetModule<IInputModule>();
            jumpBtn = inputModule.Bind(KeyCode.Space);
            slideBtn = inputModule.Bind(KeyCode.S);
            diveBtn = inputModule.Bind(KeyCode.W);
            hurtBtn = inputModule.Bind(KeyCode.T);
        }
        
    }
}