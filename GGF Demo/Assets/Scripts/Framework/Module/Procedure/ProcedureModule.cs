using System;
using System.Collections.Generic;
using System.Linq;

namespace Framework
{
    public class ProcedureModule:BaseModule,IProcedureModule
{
    private IFSM<IProcedureModule> m_fsm;
    private BaseProcedure m_currentProcedure;
    private float m_currentProcedureTime;

    public ProcedureModule()
    {
        m_fsm = null;
        m_currentProcedure = null;
        m_currentProcedureTime = 0f;
    }
    public override int Priority
    {
        get
        {
            return 3;
        }
    }

    public BaseProcedure CurrentProcedure
    {
        get
        {
            return m_currentProcedure;
        }
    }

    public float CurrentProcedureTime
    {
        get
        {
            return m_currentProcedureTime;
        }
    }

    public void Initialize(params BaseProcedure[] procedures)
    {
        m_fsm = SkyFrameworkEntry.GetModule<IFSMModule>().CreateFsm(this, procedures); //SIGNAL：List不支持泛型多态转换而数组支持
    }

    public void StartProcedure<T>() where T : BaseProcedure
    {
        if (m_fsm == null)
        {
            throw new Exception($"{m_fsm} is invalid!");
        }
        m_fsm.Start<T>();
    }

    public void StartProcedure(Type procedureType)
    {
        if (m_fsm == null)
        {
            throw new Exception($"{m_fsm} is invalid!");
        }
        
        m_fsm.Start(procedureType);
    }

    public bool HasProcedure(Type procedureType)
    {
        if (m_fsm == null)
        {
            throw new Exception($"{m_fsm} is invalid!");
        }

        return m_fsm.HasState(procedureType);
    }

    public bool HasProcedure<T>() where T : BaseProcedure
    {
        if (m_fsm == null)
        {
            throw new Exception($"{m_fsm} is invalid!");
        }

        return m_fsm.HasState<T>();
    }

    public BaseProcedure GetProcedure<T>() where T : BaseProcedure
    {
        if (m_fsm == null)
        {
            throw new Exception($"{m_fsm} is invalid!");
        }

        return m_fsm.GetState<T>();
    }

    public BaseProcedure GetProcedure(Type procedureType)
    {
        if (m_fsm == null)
        {
            throw new Exception($"{m_fsm} is invalid!");
        }

        return m_fsm.GetState(procedureType) as BaseProcedure; //拆箱
    }
}
}

