using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public interface IInputModule
    {
        /// <summary>
        /// 创建按键关联
        /// </summary>
        /// <param name="keyCode"></param>
        /// <returns></returns>
        InputButton Bind(KeyCode keyCode);

        /// <summary>
        /// 暂停获取一段时间
        /// </summary>
        /// <param name="seconds"></param>
        void StopForWhile(float seconds);

    }
}