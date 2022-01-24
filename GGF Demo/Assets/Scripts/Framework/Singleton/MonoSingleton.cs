using System;
using UnityEngine;

namespace Define
{
    public class MonoSingleton<T>:MonoBehaviour where T:MonoBehaviour
    {
        public static T instance;
        private void Awake()
        {
            if (instance == null)
            {
                DontDestroyOnLoad(this.gameObject);
                instance = this as T;
                Init();
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        protected virtual void Init()
        {
            
        }
    }
}