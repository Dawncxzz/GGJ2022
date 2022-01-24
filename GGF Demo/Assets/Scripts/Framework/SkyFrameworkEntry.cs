using System;
using System.Collections.Generic;

namespace Framework
{
    public class SkyFrameworkEntry
    {
        private static readonly LinkedList<BaseModule> frameworkModules = new LinkedList<BaseModule>();

        /// <summary>
        /// 所有游戏框架模块轮询
        /// </summary>
        /// <param name="elapseSeconds"></param>
        /// <param name="realElapseSeconds"></param>
        public static void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (BaseModule module in frameworkModules)
            {
                module.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        /// <summary>
        /// 关闭并清理游戏框架模块
        /// </summary>
        public static void Shutdown()
        {
            //从优先级低的向高进行Release
            for (LinkedListNode<BaseModule> current = frameworkModules.Last;
                 current != null;
                 current = current.Previous)
            {
                current.Value.Release();
            }

            frameworkModules.Clear();
            //TODO:清空引用池
        }
        
        /// <summary>
        /// 需要获取的游戏框架接口
        /// </summary>
        /// <typeparam name="T">框架接口类型</typeparam>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static T GetModule<T>() where T : class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new Exception($"You must get module by interface,but {interfaceType} is not!");
            }

            string moduleName = $"{interfaceType.Namespace}.{interfaceType.Name.Substring(1)}";
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new Exception($"Cant find {moduleName} module!");
            }

            return GetModule(moduleType) as T;
        }
        
        /// <summary>
        /// 获取对应的模块
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        public static BaseModule GetModule(Type moduleType)
        {
            foreach (BaseModule baseModule in frameworkModules)
            {
                if (baseModule.GetType() == moduleType)
                {
                    return baseModule;
                }
            }
            
            //若无法找到对应的Module则直接进行创建
            return CreateModule(moduleType);
        }

        
        /// <summary>
        /// 创建对应模块
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <exception cref="Exception"></exception>
        public static void CreateModule<T>() where T:class
        {
            Type interfaceType = typeof(T);
            if (!interfaceType.IsInterface)
            {
                throw new Exception($"You must create module by interface,but {interfaceType} is not!");
            }

            string moduleName = $"{interfaceType.Namespace}.{interfaceType.Name.Substring(1)}";
            Type moduleType = Type.GetType(moduleName);
            if (moduleType == null)
            {
                throw new Exception($"Cant find {moduleName} module!");
            }

            CreateModule(moduleType);
        }
        
        /// <summary>
        /// 创建对应模块
        /// </summary>
        /// <param name="moduleType"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static BaseModule CreateModule(Type moduleType)
        {
            BaseModule module = (BaseModule)Activator.CreateInstance(moduleType);
            if (module == null)
            {
                throw new Exception($"Can not create module '{moduleType.FullName}'");
            }
            module.LoadModule();
            
            LinkedListNode<BaseModule> current = frameworkModules.First;
            while (current != null)
            {
                if (module.Priority > current.Value.Priority)
                {
                    break;
                }

                current = current.Next;
            }

            if (current != null)
            {
                frameworkModules.AddBefore(current, module);
            }
            else
            {
                frameworkModules.AddLast(module);
            }

            return module;
        }
        
        
    }
}