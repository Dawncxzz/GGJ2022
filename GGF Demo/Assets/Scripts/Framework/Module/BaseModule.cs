using System.Collections;
using System.Collections.Generic;
using Define;
using Framework;
using UnityEngine;

/*
 * 具体Module实现时必须有对应的IXXX接口，方便封装具体功能
 */
public class BaseModule
{
    /// <summary>
    /// 游戏框架模块Update优先级，优先级高的优先进行轮询
    /// </summary>
    public virtual int Priority
    {
        get
        {
            return 0;
        }
    }
    public enum EnumRegisterMode
    {
        NotRegister,
        AlreadyRegister
    }

    private EnumObjectState state = EnumObjectState.Initial;
    public event StateChangedHandler OnStateChange;
    private EnumRegisterMode registerMode;
    
    public EnumObjectState State
    {
        get
        {
            return state;
            
        }
        set
        {
            //MARKER:OnStateChange
            if(state==value) return;

            EnumObjectState oldState = state;
            state = value;
            if (OnStateChange != null)
            {
                OnStateChange(this, oldState, state);
            }
        }
    }

    public bool HasRegister
    {
        get
        {
            return registerMode == EnumRegisterMode.AlreadyRegister;
        }
    }

    public void LoadModule()
    {
        if(state!=EnumObjectState.Initial) return;
        State = EnumObjectState.Loading;

        OnLoadModule();
        State = EnumObjectState.Enable;
        registerMode = EnumRegisterMode.AlreadyRegister;
    }

    protected virtual void OnLoadModule()
    {
        
    }

    public void Release()
    {
        if (state == EnumObjectState.Destroy) return;
        State = EnumObjectState.Destroy;
        OnRelease();
        registerMode = EnumRegisterMode.NotRegister;
    }

    protected virtual void OnRelease()
    {
        
    }

    public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
    {
        
    }
}
