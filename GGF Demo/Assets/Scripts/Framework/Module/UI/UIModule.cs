using System;
using System.Collections.Generic;
using System.Linq;
using Define;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Object = UnityEngine.Object;

namespace Framework
{
    public class UIModule:BaseModule,IUIModule
    {
        private HashSet<WindowID> visibleWindows = new HashSet<WindowID>();
        private HashSet<WindowID> windowsInControl = new HashSet<WindowID>();
        private Dictionary<int, UIBaseWindow> allWindowsDic = new Dictionary<int, UIBaseWindow>();
        private Queue<WindowID> windowQueue = new Queue<WindowID>(); //后期用作特殊的UIWindowType处理数据
        private Dictionary<int, Transform> uiTypeDic = new Dictionary<int, Transform>();
        private Dictionary<int, Type> windowId2Type = new Dictionary<int, Type>();


        public override int Priority
        {
            get
            {
                return 0;
            }
        }

        private void InitUIInstance()
        {
            if(GameEntry.instance.transform.Find("UIModule")!=null) return;
            
            GameObject prefab=Resources.Load<GameObject>("UI/UIModule");
            GameObject go=GameObject.Instantiate(prefab,GameEntry.instance.transform);
            go.name = "UIModule";

            int index = 0;
            foreach (var name in Enum.GetNames(typeof(UIWindowType)))
            {
                this.uiTypeDic.Add(index++,go.transform.Find(name));
            }
            
            //MARKER:设置相机叠加
            Camera uiCamera = go.transform.Find("UICamera").GetComponent<Camera>();
            Camera.main.GetUniversalAdditionalCameraData().cameraStack.Add(uiCamera);
        }

        private void RemoveUIInstance()
        {
            Transform transform = GameEntry.instance.transform.Find("UIModule");
            if (transform != null)
            {
                GameObject.Destroy(transform.gameObject);
            }
        }

        private void InitType()
        {
            string[] names = Enum.GetNames(typeof(WindowID));
            for (int i = 1; i < names.Length; i++)
            {
                string typeClass = $"Logic.Sky{names[i].Split('_')[1]}";
                Type type = Type.GetType(typeClass);
                if (type != null&&typeof(UIBaseWindow).IsAssignableFrom(type))
                {
                    this.windowId2Type.Add(i,type);
                }
                
            }
        }

        protected override void OnLoadModule()
        {
            InitUIInstance();
            InitType();
        }

        protected override void OnRelease()
        {
            RemoveUIInstance();
        }

        private UIBaseWindow GetUIBaseWindow(WindowID id)
        {
            if (this.IsWindowInControl(id))
            {
                return this.allWindowsDic[(int)id];
            }

            return null;
        }

        private Transform GetRootByUIWindowType(UIWindowType windowType)
        {
            return this.uiTypeDic[(int)windowType];
        }

        private UIBaseWindow ReadyToShowUI(WindowID id)
        {
            UIBaseWindow uibaseWindow = this.GetUIBaseWindow(id);
            if (uibaseWindow != null)
            {
                if (!uibaseWindow.IsPreload)
                {
                    uibaseWindow.Load();
                    uibaseWindow.SetRoot(GetRootByUIWindowType(uibaseWindow._UIWindowType));
                }
            }
            else 
            {
                if (!this.windowId2Type.ContainsKey((int)id))
                {
                    throw new Exception($"Cant find ui class:{id}!");
                }
                uibaseWindow = Activator.CreateInstance(this.windowId2Type[(int)id]) as UIBaseWindow;
                uibaseWindow.Load();
                uibaseWindow.SetRoot(GetRootByUIWindowType(uibaseWindow._UIWindowType));
                
                this.windowsInControl.Add(uibaseWindow._WindowID);
                this.allWindowsDic.Add((int)(uibaseWindow._WindowID),uibaseWindow);
            }

            
            return uibaseWindow;
        }

        private void RealShowWindow(UIBaseWindow window,params object[] _params)
        {
            if (window._UIWindowType == UIWindowType.Popup)
            {
                windowQueue.Enqueue(window._WindowID);
            }
            
            if (null != window.uiTransform)
            {
                window.uiTransform.gameObject.SetActive(true);
            }
            if (!window.isShowed)
            {
                window.OnStart(_params);
            }
            else
            {
                window.OnResume(_params);
            }
            window.OnShow(_params);
            this.visibleWindows.Add(window._WindowID);
            window.isShowed = true;
            window.isShowing = true;
            
            window.uiTransform.SetAsLastSibling();
        }

        public void ShowUIByID(WindowID id,params object[] _params)
        {
            UIBaseWindow uibaseWindow = this.ReadyToShowUI(id);
            if (null != uibaseWindow)
            {
                this.RealShowWindow(uibaseWindow,_params);
            }
        }
        

        public void ShowUIByType(UIBaseWindow uiBaseWindow)
        {
            ShowUIByID(uiBaseWindow._WindowID);
        }

        private bool CheckHide(WindowID id)
        {
            if (!this.IsWindowInControl(id))
            {
                Debug.Log("#Current UI Manager has no control power of"+id);
                return false;
            }

            if (!this.visibleWindows.Contains(id))
            {
                return false;
            }

            UIBaseWindow baseWindow = this.GetUIBaseWindow(id);
            if (null != baseWindow.uiTransform)
            {
                baseWindow.uiTransform.gameObject.SetActive(false);
            }
            
            baseWindow.OnHide();
            this.visibleWindows.Remove(id);
            baseWindow.isShowing = false;
            return true;
        }

        public void HideUIByID(WindowID id, bool showNextUI=true)
        {
            if (!this.CheckHide(id))
            {
                Debug.LogWarning($"检测关闭 WindowID:{id}失败");
                return;
            }

            if (!showNextUI)
            {
                return;
            }

            if (!this.IsWindowInControl(id))
            {
                return;
            }

            UIBaseWindow uibaseWindow = this.GetUIBaseWindow(id);
            WindowID preWindowID = uibaseWindow._PreWindowID;
            if (preWindowID != WindowID.WindowID_Invalid)
            {
                this.ShowUIByID(preWindowID);
            }
        }

        public void HideUIByType(UIBaseWindow uiBaseWindow, bool showNextUI=true)
        {
            HideUIByID(uiBaseWindow._WindowID);
        }

        private void UnloadUI(WindowID id)
        {
            UIBaseWindow uibaseWindow = this.GetUIBaseWindow(id);
            if (null != uibaseWindow)
            {
                if (uibaseWindow.IsPreload)
                {
                    uibaseWindow.Unload();
                }
            }

            this.allWindowsDic.Remove((int)id);
            this.windowsInControl.Remove(id);
        }

        public void CloseUIByID(WindowID id, bool showNextUI=true)
        {
            if (!this.IsWindowInControl(id))
            {
                Debug.Log("#Current UI Manager has no control power of"+id);
                return;
            }

            if (!this.visibleWindows.Contains(id))
            {
                return;
            }
            
            this.HideUIByID(id);
            this.UnloadUI(id);
            
        }

        public void CloseUIByType(UIBaseWindow uiBaseWindow, bool showNextUI)
        {
            CloseUIByID(uiBaseWindow._WindowID);
        }

        public void HideAndShowUI(WindowID hideId, WindowID showId)
        {
            HideUIByID(hideId,false);
            ShowUIByID(showId,hideId);
            UIBaseWindow uibaseWindow = this.GetUIBaseWindow(showId);
            if (uibaseWindow != null)
            {
                uibaseWindow._PreWindowID = hideId;
            }
        }

        public bool IsVisible(WindowID windowID)
        {
            return this.visibleWindows.Contains(windowID);
        }

        public bool IsWindowInControl(WindowID windowID)
        {
            return this.windowsInControl.Contains(windowID);
        }

        public void HideLastWindow()
        {
            if (visibleWindows.Count <= 0)
            {
                return;
            }

            WindowID windowID = visibleWindows.Last();
            if (!IsVisible(windowID))
            {
                return;
            }
            HideUIByID(windowID);
        }
    }
}