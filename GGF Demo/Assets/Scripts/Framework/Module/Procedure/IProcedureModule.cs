using System;

namespace Framework
{
    public interface IProcedureModule
    {
        
        /// <summary>
        /// 获取当前流程
        /// </summary>
        BaseProcedure CurrentProcedure
        {
            get;
        }

        /// <summary>
        /// 获取当前流程持续时间
        /// </summary>
        float CurrentProcedureTime
        {
            get;
        }

        /// <summary>
        /// 初始化流程模块
        /// </summary>
        /// <param name="procedures"></param>
        void Initialize(params BaseProcedure[] procedures);

        /// <summary>
        /// 开启流程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        void StartProcedure<T>() where T : BaseProcedure;


        /// <summary>
        /// 开启流程
        /// </summary>
        /// <param name="procedureType"></param>
        void StartProcedure(Type procedureType);


        /// <summary>
        /// 是否有对应流程
        /// </summary>
        /// <param name="procedureType"></param>
        bool HasProcedure(Type procedureType);


        /// <summary>
        /// 是否有对应流程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        bool HasProcedure<T>() where T : BaseProcedure;
        
        
        /// <summary>
        /// 获取流程
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        BaseProcedure GetProcedure<T>() where T:BaseProcedure;


        /// <summary>
        /// 获取流程
        /// </summary>
        /// <param name="procedureType"></param>
        /// <returns></returns>
        BaseProcedure GetProcedure(Type procedureType);

    }
}

