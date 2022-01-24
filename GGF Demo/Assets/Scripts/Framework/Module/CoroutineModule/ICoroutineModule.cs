using System;
using System.Collections;

namespace Framework
{
    public interface ICoroutineModule
    {
        /// <summary>
        /// 创建协程
        /// </summary>
        void CreateCoroutine(IEnumerator coroutine, Action callback = null);
    }
}

