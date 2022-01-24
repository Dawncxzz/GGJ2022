using System;
using System.Collections;
using System.Collections.Generic;
using Define;
using Framework;
using Logic;
using UnityEngine;

namespace Framework
{
    public class GameEntry : MonoSingleton<GameEntry>
    {
        private void Start()
        {
            InitModules();
            StartProcedure();
        }

        private void InitModules()
        {
            SkyFrameworkEntry.CreateModule<IUIModule>();
            SkyFrameworkEntry.CreateModule<IFSMModule>();
            SkyFrameworkEntry.CreateModule<IProcedureModule>();
            SkyFrameworkEntry.CreateModule<IResourcesModule>();
            SkyFrameworkEntry.CreateModule<IInputModule>();
        }
        
        private void StartProcedure()
        {
            IProcedureModule procedureModule = SkyFrameworkEntry.GetModule<IProcedureModule>();
            procedureModule.Initialize(new BaseProcedure[]{new InitGameProcedure(),new EnterGameProcedure(),new EndGameProcedure()});
            procedureModule.StartProcedure<InitGameProcedure>();
        }

        private void Update()
        {
            SkyFrameworkEntry.Update(Time.deltaTime,Time.unscaledDeltaTime);
        }
    }

}

