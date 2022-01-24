using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Framework
{
    public class CoroutineManager:MonoBehaviour
    {
        
        
        public void CreateCoroutine(IEnumerator coroutine,Action callback)
        {
           StartCoroutine(outCouroutine(coroutine,callback));
           callback();
        }

        public void RemoveCoroutine(IEnumerator coroutine)
        {
            StopCoroutine(coroutine);
        }

        IEnumerator outCouroutine(IEnumerator coroutine,Action callback)
        {
            yield return coroutine;
            callback();
        }
    }
}

