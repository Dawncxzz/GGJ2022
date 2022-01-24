using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using System.Collections;



namespace Framework
{
    public class ScenesSwitch
    {
        public static AsyncOperation AsyncOperation { get; private set; }

        public static float LoadPro { get => AsyncOperation.progress; }


        /// <summary>
        /// 根据场景名切换场景
        /// </summary>
        /// <param name="name"></param>
        public static IEnumerator LoadScene(string name)
        {
            AsyncOperation = SceneManager.LoadSceneAsync(name);
            AsyncOperation.allowSceneActivation = false;
            yield return AsyncOperation;
        }



    }
}
