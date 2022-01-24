using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Framework
{
    public class ChangeSceneByButton:MonoBehaviour
    {
        Button Button;
        public Text Text;
        string SceneName;
        int count = 0;
        int max = 0;
        bool isLoading = false;

        private void Start()
        {
            Button = GetComponent<Button>();
            SceneName = "Test";
            Button.onClick.AddListener(() => { EventChangeScene(); });
        }

        public void EventChangeScene()
        {
            StartCoroutine(ScenesSwitch.LoadScene(SceneName));
            isLoading = true;
        }

        private void Update()
        {
            if (isLoading)
            {
                if (ScenesSwitch.AsyncOperation.progress < 0.9f)
                {
                    max = (int)(100 * ScenesSwitch.AsyncOperation.progress / 0.9f);
                    if (count < max)
                    {
                        count++;
                    }
                    Text.text = count.ToString();
                }
                else if (ScenesSwitch.AsyncOperation.progress >= 0.9f)
                {
                    count = 100;
                    Text.text = count.ToString();
                    ScenesSwitch.AsyncOperation.allowSceneActivation = true;
                }
            }
        }

    }
}
