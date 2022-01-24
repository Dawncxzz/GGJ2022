using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Define;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Framework
{
    public class ResourcesModule:BaseModule,IResourcesModule
    {
        public AssetBundleManifest AssetBundleManifest { get; set; }

        private readonly Dictionary<string, ABInfo> bundles =
            new Dictionary<string, ABInfo>();

        private readonly Dictionary<string, Dictionary<string, Object>> resourcesCache =
            new Dictionary<string, Dictionary<string, Object>>();

        private readonly Dictionary<string, string[]> dependenciesCache = new Dictionary<string, string[]>();
        protected override void OnLoadModule()
        {
            // LoadOneBundle("StreamingAssets");
            // AssetBundleManifest=(AssetBundleManifest)GetAsset("StreamingAssets", "AssetBundleManifest");
        }

        protected override void OnRelease()
        {
           
        }

        public bool Contains(string bundleName)
        {
            return this.bundles.ContainsKey(bundleName);
        }

        // private string bundleNameToLower(string bundleName)
        // {
        //     if()
        // }

        public Dictionary<string, Object> GetBundleAll(string bundleName)
        {
            Dictionary<string, Object> cache;
            if (this.resourcesCache.TryGetValue(bundleName.ToLower(), out cache))
            {
                throw new Exception($"Cant find bundle:{bundleName.ToLower()}!");
            }

            return cache;
        }

        public Object GetAsset(string bundleName, string prefab)
        {
            Dictionary<string, Object> cache;
            if (!this.resourcesCache.TryGetValue(bundleName.ToLower(), out cache))
            {
                throw new Exception($"Cant find bundle:{bundleName.ToLower()}!");
            }

            Object resources = null;
            if (!cache.TryGetValue(prefab, out resources))
            {
                throw new Exception($"Cant find asset:{bundleName.ToLower()}!");
            }

            return resources;
        }
        

        public void UnloadBundleAsync(string assetBundleName, bool unload = true,Action callback=null)
        {
            SkyFrameworkEntry.GetModule<ICoroutineModule>().CreateCoroutine(OnUnloadBundleAsync(assetBundleName,unload),callback);
           
        }

        private IEnumerator OnUnloadBundleAsync(string assetBundleName, bool unload = true)
        {
            assetBundleName = assetBundleName.ToLower();

            string[] dependencies = GetSortedDependencies(assetBundleName);
            foreach (var dependency in dependencies)
            {
                this.UnloadOneBundle(dependency, unload);
                yield return 1;
            }
        }

        public void UnloadBundle(string assetBundleName, bool unload = true)
        {
            assetBundleName = assetBundleName.ToLower();
            string[] dependencies = GetSortedDependencies(assetBundleName);
            foreach (string dependency in dependencies)
            {
                this.UnloadOneBundle(dependency, unload);
            }
        }

        private void UnloadOneBundle(string assetBundleName, bool unload = true)
        {
            assetBundleName = assetBundleName.ToLower();
            ABInfo abInfo;
            if (!this.bundles.TryGetValue(assetBundleName, out abInfo))
            {
                return;
            }
            if (--abInfo.RefCount > 0)
            {
                return;
            }

            this.bundles.Remove(assetBundleName);
            this.resourcesCache.Remove(assetBundleName);
            abInfo.Destroy(unload);
        }

        public void LoadBundle(string assetBundleName)
        {
            assetBundleName = assetBundleName.ToLower();
            string[] dependencies = GetSortedDependencies(assetBundleName);
            foreach (var dependency in dependencies)
            {
                if (string.IsNullOrEmpty(dependency))
                {
                    continue;
                }
                
                this.LoadOneBundle(dependency);
            }
        }

        private void LoadOneBundle(string assetBundleName)
        {
            if(IsBundleLoaded(assetBundleName)) return;

            assetBundleName = assetBundleName.ToLower();
            ABInfo abInfo;
            if (this.bundles.TryGetValue(assetBundleName, out abInfo))
            {
                ++abInfo.RefCount;
                return;
            }

            // if (Define.Define.IsEditor)
            // {
            //     string[] realPath = null;
            //     realPath = AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            //     foreach (string s in realPath)
            //     {
            //         string assetName = Path.GetFileNameWithoutExtension(s);
            //         UnityEngine.Object resource = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
            //         AddResource(assetBundleName, assetName, resource);
            //     }
            //
            //     if (realPath.Length > 0)
            //     {
            //         abInfo = new ABInfo(assetBundleName, null);
            //         this.bundles[assetBundleName] = abInfo;
            //     }
            //     else
            //     {
            //         Debug.LogWarning($"assets bundle not found: {assetBundleName}");
            //     }
            //     
            //     return;
            // }

            string p = Path.Combine(ABDefine.ABPath, assetBundleName);
            AssetBundle assetBundle = null;
            if (File.Exists(p))
            {
                assetBundle=AssetBundle.LoadFromFile(p);
            }

            if (assetBundle == null)
            {
                Debug.LogWarning($"assetBundle not found:{assetBundleName}!");
                return;
            }
            
            //判断是否是流式场景
            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                var assets = assetBundle.LoadAllAssets();
                foreach (var asset in assets)
                {
                    AddResource(assetBundleName,asset.name,asset);
                }
            }

            abInfo = new ABInfo(assetBundleName, assetBundle);
            bundles[assetBundleName] = abInfo;

        }

        private void AddResource(string bundleName, string assetName, Object resource)
        {
            Dictionary<string, Object> dict;
            if (!this.resourcesCache.TryGetValue(bundleName.ToLower(), out dict))
            {
                dict = new Dictionary<string, Object>();
                this.resourcesCache[bundleName] = dict;
                
            }

            dict[assetName] = resource;
        }

        private bool IsBundleLoaded(string assetBundleName)
        {
            return resourcesCache.ContainsKey(assetBundleName);
        }

        private string[] GetSortedDependencies(string assetBundleName)
        {
            var info = new Dictionary<string, int>();
            var parents = new List<string>();
            CollectDependencies(parents,assetBundleName,info);
            string[] result = info.OrderBy(x => x.Value).Select(x => x.Key).ToArray();
            return result;
        }

        private void CollectDependencies(List<string> parents, string assetBundleName, Dictionary<string, int> info)
        {
            parents.Add(assetBundleName);
            string[] depe = GetDependencies(assetBundleName);
            foreach (var parent in parents)
            {
                if (!info.ContainsKey(parent))
                {
                    info[parent] = 0;//记录依赖次数
                }

                info[parent] += depe.Length;
                
            }

            foreach (string dep in depe)
            {
                if (parents.Contains(dep))
                {
                    throw new Exception($"包有循环依赖：{assetBundleName}:{dep}!");
                }
                
                CollectDependencies(parents,dep,info);
            }

            parents.RemoveAt(parents.Count - 1); //当前assetBundle的所有依赖查询完成
        }

        private string[] GetDependencies(string assetBundleName)
        {
            string[] dependencies = null;
            if (dependenciesCache.TryGetValue(assetBundleName, out dependencies))
            {
                return dependencies;
            }
            // if (Define.Define.IsEditor)
            // {
            //     dependencies = AssetDatabase.GetAssetBundleDependencies(assetBundleName, true);
            // }
            //else
            //{
                dependencies = this.AssetBundleManifest.GetAllDependencies(assetBundleName);
            //}

            
            dependenciesCache.Add(assetBundleName,dependencies);
            return dependencies;

        }

        public void LoadBundleAsync(string assetBundleName,Action callback=null)
        {
            SkyFrameworkEntry.GetModule<ICoroutineModule>().CreateCoroutine(OnLoadBundleAsync(assetBundleName),callback);
        }

        private IEnumerator OnLoadBundleAsync(string assetBundleName)
        {
            assetBundleName = assetBundleName.ToLower();

            string[] dependencies = GetSortedDependencies(assetBundleName);
            yield return LoadOneBundleAsync(assetBundleName);

            foreach (var dependency in dependencies)
            {
                yield return LoadOneBundleAsync(dependency);
               
            }
        }

        private IEnumerator LoadOneBundleAsync(string assetBundleName)
        {
            if (IsBundleLoaded(assetBundleName)) yield break;

            assetBundleName = assetBundleName.ToLower();
            ABInfo abInfo;

            if (this.bundles.TryGetValue(assetBundleName, out abInfo))
            {
                ++abInfo.RefCount;
                yield break;
            }

            string p = "";
            AssetBundle assetBundle = null;
            
            
            // if (Define.Define.IsEditor)
            // {
            //     string[] realPath = UnityEditor.AssetDatabase.GetAssetPathsFromAssetBundle(assetBundleName);
            //     foreach (string s in realPath)
            //     {
            //         string assetName = Path.GetFileNameWithoutExtension(s);
            //         UnityEngine.Object resource = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(s);
            //         AddResource(assetBundleName, assetName, resource);
            //     }
            //
            //     if (realPath.Length > 0)
            //     {
            //         abInfo = new ABInfo(assetBundleName, null);
            //         this.bundles[assetBundleName] = abInfo;
            //        
            //     }
            //     else
            //     {
            //         Debug.LogError("Bundle not exist! BundleName: " + assetBundleName);
            //     }
            //
            //     yield return new WaitForSeconds(0.1f);
            //     yield break;
            // }
            
            p = Path.Combine(ABDefine.ABPath, assetBundleName);
            if (!File.Exists(p))
            {
                Debug.LogWarning($"not find {assetBundleName} !");
            }
            
            AssetBundleCreateRequest request=AssetBundle.LoadFromFileAsync(p);
            yield return request;
            assetBundle = request.assetBundle;
            
            if (assetBundle == null)
            {
                // 获取资源的时候会抛异常，这个地方不直接抛异常，因为有些地方需要Load之后判断是否Load成功
                Debug.LogWarning($"assets bundle not found: {assetBundleName}");
            }
            
            if (!assetBundle.isStreamedSceneAssetBundle)
            {
                var assets = assetBundle.LoadAllAssets();
                foreach (var asset in assets)
                {
                    AddResource(assetBundleName,asset.name,asset);
                }
            }
            
            abInfo = new ABInfo(assetBundleName, assetBundle);
            this.bundles[assetBundleName] = abInfo;
            
        }
        
    }
}

