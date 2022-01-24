using System;
using UnityEngine;

namespace Define
{
    //MARKER:单例抽象类(要求T为类且具有无参构造函数)
    public abstract class Singleton<T> where T:class,new()
    {
        protected static T _instance = null;

        private Transform transform;
        //MARKER:属性代替getInstance()
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();
                }

                return _instance;
            }
        }

        protected Singleton()
        {
            if (_instance != null)
            {
                throw new Exception("Already have that instance");
            }
            Init();
        }

        protected virtual void Init()
        {
        
        }
    }
}