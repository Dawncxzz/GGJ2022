namespace Define
{
    public static class Define
    {
#if UNITY_EDITOR
        public static bool IsEditor = true;
#else
        public static bool IsEditor = false;
#endif
    }

    #region Global delegate

    public delegate void StateChangedHandler(object sender,EnumObjectState oldState,EnumObjectState curState);

    #endregion
    
    public enum EnumObjectState
    {
        Initial,
        Loading,
        Enable,
        Disable,
        Destroy,
    }
}