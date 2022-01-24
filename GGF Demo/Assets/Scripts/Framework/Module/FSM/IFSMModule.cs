using System;
using System.Collections.Generic;

namespace Framework
{
    public interface IFSMModule
{
    /// <summary>
    /// 获取有限状态机数量
    /// </summary>
    int Count
    {
        get;
    }

    /// <summary>
    /// 检查是否存在有限状态机
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    bool HasFSM<T>() where T : class;

    /// <summary>
    /// 检查是否存在有限状态机
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    bool HasFSM<T>(string name) where T : class;
    


    /// <summary>
    /// 检查是否存在有限状态机
    /// </summary>
    /// <param name="ownerType"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    bool HasFSM(Type ownerType, string name);


    /// <summary>
    /// 获取状态机
    /// </summary>
    /// <returns></returns>
    IFSM<T> GetFSM<T>() where T : class;

    /// <summary>
    /// 获取状态机
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    FSMBase GetFSM(Type type);

    /// <summary>
    /// 获取状态机
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IFSM<T> GetFSM<T>(string name) where T : class;

    /// <summary>
    /// 获取状态机
    /// </summary>
    /// <param name="ownerType"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    FSMBase GetFSM(Type ownerType, string name);


    /// <summary>
    /// 获取所有状态机
    /// </summary>
    /// <returns></returns>
    FSMBase[] GetAllFSM();
    
    /// <summary>
    /// 创建有限状态机。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="owner"></param>
    /// <param name="states"></param>
    /// <returns></returns>
    IFSM<T> CreateFsm<T>(T owner, List<FSMState<T>> states) where T : class;

    /// <summary>
    /// 创建有限状态机
    /// </summary>
    /// <param name="owner"></param>
    /// <param name="states"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    IFSM<T> CreateFsm<T>(T owner, params FSMState<T>[] states) where T : class;

    /// <summary>
    /// 创建有限状态机。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <param name="owner"></param>
    /// <param name="states"></param>
    /// <returns></returns>
    IFSM<T> CreateFsm<T>(string name, T owner, List<FSMState<T>> states) where T : class;
    
    
    
    /// <summary>
    /// 销毁有限状态机。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="name"></param>
    /// <returns></returns>
    bool DestroyFsm<T>(string name) where T : class;

    /// <summary>
    /// 销毁有限状态机。
    /// </summary>
    /// <param name="ownerType"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    bool DestroyFsm(Type ownerType, string name);
}
}

