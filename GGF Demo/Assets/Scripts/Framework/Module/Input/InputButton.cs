using UnityEngine;

namespace Framework
{
    public class InputButton
    {
        public KeyCode keyCode;
        public bool curState = false;
        public bool lastState = false;
        public bool isPressing = false;
        //后面根据情况加入检测
        public bool isPressed = false;
        

        public InputButton(KeyCode code)
        {
            keyCode = code;
        }
        
        public void Check(bool value)
        {
            curState = value;
            isPressing = value;
            isPressed = false;
            if (curState != lastState)
            {
                isPressed = curState;
            }
            
            lastState = curState;
        }
    }
}