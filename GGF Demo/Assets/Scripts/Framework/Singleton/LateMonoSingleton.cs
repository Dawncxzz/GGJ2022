using UnityEngine;

namespace Define
{
    //MARKER:延迟加载MonoBehavior的单例抽象基类
    public abstract class LateMonoSingleton<T>:MonoBehaviour where T:MonoBehaviour
    {
        protected static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = GameObject.FindObjectOfType<T>();
                    if (_instance == null)
                    {
                        GameObject DDOL = GameObject.Find("DontDestroyOnLoadSingleton");
                        if (DDOL == null)
                        {
                            DDOL = new GameObject("DontDestroyOnLoadSingleton");
                            DontDestroyOnLoad(DDOL);
                        }

                        GameObject go = new GameObject(typeof(T).ToString());
                        go.transform.SetParent(DDOL.transform,false);
                        _instance = go.AddComponent<T>();
                    }
                    else if(_instance.transform.parent.name!="DontDestroyOnLoadSingleton")
                    {
                        GameObject DDOL = GameObject.Find("DontDestroyOnLoadSingleton");
                        if (DDOL == null)
                        {
                            DDOL = new GameObject("DontDestroyOnLoadSingleton");
                            DontDestroyOnLoad(DDOL);
                        }
                        
                        _instance.transform.SetParent(DDOL.transform,false);
                    }
                }
                return _instance;
            }
        }
    }
}