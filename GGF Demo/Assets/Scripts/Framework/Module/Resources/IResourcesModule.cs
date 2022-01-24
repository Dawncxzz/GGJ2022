using System;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public class ABInfo
    {
        public string Name { get; set; }
        
        public int RefCount { get; set; }

        public AssetBundle assetBundle;

        public bool AlreadyLoadAssets;

        public ABInfo(string name, AssetBundle assetBundle)
        {
            this.Name = name;
            this.assetBundle = assetBundle;
        }

        public void Destroy(bool unload = true)
        {
            if (this.assetBundle != null)
            {
                this.assetBundle.Unload(unload);
            }
        }
    }
    public interface IResourcesModule
    {
        AssetBundleManifest AssetBundleManifest { get; set; }


        /// <summary>
        /// 是否包含对应bundle
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        bool Contains(string bundleName);

        /// <summary>
        /// 获取Bundle所有缓存
        /// </summary>
        /// <param name="bundleName"></param>
        /// <returns></returns>
        Dictionary<string, Object> GetBundleAll(string bundleName);


        /// <summary>
        /// 加载asset
        /// </summary>
        /// <param name="bundleName"></param>
        /// <param name="prefab"></param>
        /// <returns></returns>
        Object GetAsset(string bundleName, string prefab);


        /// <summary>
        /// 异步卸载Bundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="unload"></param>
        void UnloadBundleAsync(string assetBundleName, bool unload = true,Action callback=null);

        /// <summary>
        /// 卸载Bundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        /// <param name="unload"></param>
        void UnloadBundle(string assetBundleName, bool unload = true);


        /// <summary>
        /// 加载bundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        void LoadBundle(string assetBundleName);

        /// <summary>
        /// 异步加载bundle
        /// </summary>
        /// <param name="assetBundleName"></param>
        void LoadBundleAsync(string assetBundleName,Action callback=null);
    }
}

