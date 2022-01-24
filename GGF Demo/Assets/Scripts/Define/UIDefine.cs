using System.Collections.Generic;

namespace Define
{
    public enum WindowID
    {
        WindowID_Invalid,
        WindowID_Menu,
        WindowID_Property, //角色属性
        WindowID_Settle, //结算页面
    }

    public enum UIWindowType
    {
        Normal,
        Other,
        Popup,
    }

    public static class UIDefine
    {
        public static Dictionary<WindowID, string> id2Path = new Dictionary<WindowID, string>()
        {
            { WindowID.WindowID_Menu ,"UI/SkyMenu"},
            { WindowID.WindowID_Settle,"UI/SkySettle"},
            {WindowID.WindowID_Property,"UI/SkyProperty"},
        };
        
    }
}