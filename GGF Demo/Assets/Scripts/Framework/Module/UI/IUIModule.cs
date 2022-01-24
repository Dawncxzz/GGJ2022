using System.Collections.Generic;
using Define;

namespace Framework
{
    public interface IUIModule
    {

        /// <summary>
        /// 展示UI
        /// </summary>
        void ShowUIByID(WindowID id,params object[] _params);
        
        /// <summary>
        /// 展示UI
        /// </summary>
        void ShowUIByType(UIBaseWindow uiBaseWindow);

        /// <summary>
        /// 隐藏UI
        /// </summary>
        void HideUIByID(WindowID id,bool showNextUI);

        /// <summary>
        /// 隐藏UI
        /// </summary>
        void HideUIByType(UIBaseWindow uiBaseWindow,bool showNextUI);

        /// <summary>
        /// 关闭UI
        /// </summary>
        void CloseUIByID(WindowID id,bool showNextUI);

        /// <summary>
        /// 关闭UI
        /// </summary>
        void CloseUIByType(UIBaseWindow uiBaseWindow,bool showNextUI);

        /// <summary>
        /// 隐藏并展示新UI
        /// </summary>
        /// <param name="hideId"></param>
        /// <param name="showId"></param>
        void HideAndShowUI(WindowID hideId, WindowID showId);
        
        /// <summary>
        /// 查询是否处在显示状态
        /// </summary>
        /// <returns></returns>
        bool IsVisible(WindowID windowID);

        /// <summary>
        /// 是否处在管理状态
        /// </summary>
        /// <param name="windowID"></param>
        /// <returns></returns>
        bool IsWindowInControl(WindowID windowID);

        /// <summary>
        /// 隐藏PopUp的最后一个Window
        /// </summary>
        void HideLastWindow();
    }
}