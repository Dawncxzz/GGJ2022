using System;
using Define;
using UnityEngine;
using Object = System.Object;

namespace Framework
{
    public abstract class UIBaseWindow
    {
        public abstract WindowID _WindowID
        {
            get;
        }

        public abstract UIWindowType _UIWindowType
        {
            get;
        }

        public bool isShowed
        {
            get { return _isShowed; }
            set { _isShowed = value; }
        }

        private bool _isShowed;

        public bool isShowing
        {
            get { return _isShowing; }
            set { _isShowing = value; }
        }

        private bool _isShowing;

        public  WindowID _PreWindowID
        {
            get { return m_preWindowID; }
            set { m_preWindowID = value; }
        }

        private WindowID m_preWindowID;

        public Transform uiTransform
        {
            get { return m_uiPrefabGameObject.transform; }
        }

        public bool IsPreload
        {
            get { return this.m_uiPrefabGameObject != null; }
        }
        
        private GameObject m_uiPrefabGameObject = null;

        /// <summary>
        /// 加载时调用
        /// </summary>
        public abstract void OnLoad();

        /// <summary>
        /// 加载时调用，加入监听
        /// </summary>
        public abstract void OnRegisterListener();
        
        /// <summary>
        /// 初次展示时调用
        /// </summary>
        public abstract void OnStart(params object[] _params);

        /// <summary>
        /// 恢复显示时调用
        /// </summary>
        public abstract void OnResume(params object[] _params);

        /// <summary>
        /// 展示时调用
        /// </summary>
        public abstract void OnShow(params object[] _params);

        /// <summary>
        /// 隐藏时调用
        /// </summary>
        public abstract void OnHide();

        /// <summary>
        /// 关闭时调用
        /// </summary>
        public abstract void OnUnload();

        /// <summary>
        /// 加载UI
        /// </summary>
        public void Load()
        {
            if (!UIDefine.id2Path.ContainsKey(this._WindowID))
            {
                throw new Exception($"{_WindowID} is not exist!");
            }
            
            GameObject prefab = Resources.Load<GameObject>(UIDefine.id2Path[_WindowID]);
            
            //MARKER:改成AB加载
            // string abName = "Sky"+_WindowID.ToString().Split('_')[1];
            //
            // IResourcesModule resourcesModule = SkyFrameworkEntry.GetModule<IResourcesModule>();
            // resourcesModule.LoadBundle(AssetBundleHelper.StringToAB(abName));
            // GameObject prefab=resourcesModule.GetAsset(AssetBundleHelper.StringToAB(abName), abName) as GameObject;
            
            this.m_uiPrefabGameObject = GameObject.Instantiate(prefab);
            this.OnLoad();
            this.OnRegisterListener();
        }

        /// <summary>
        /// 异步加载UI
        /// </summary>
        public void LoadAsync()
        {
            //todo:
        }

        public void Unload()
        {
            if (this.m_uiPrefabGameObject != null)
            {
                this.OnUnload();
                GameObject.Destroy(this.m_uiPrefabGameObject);
                this.m_uiPrefabGameObject = null;
                Resources.UnloadUnusedAssets();
            }
        }
        
        /// <summary>
        /// 设置父层级GO
        /// </summary>
        public void SetRoot(Transform rootTransform)
        {
            if (null != rootTransform && null != uiTransform)
            {
                uiTransform.SetParent(rootTransform, false);
            }
        }
    }
}