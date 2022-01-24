namespace Define
{
    public static class GameDefine
    {
        public static float ObstacleSpeed = 8f; //障碍物速度
        public static float MapGravity = 21.33f; //地图重力加速度
        public static float JumpSpeed = 7f; //跳跃速度
        public static float DiveSpeed = 12f;//飞扑速度
        public static float PlayerWidth = 1.5f; //人物宽度
        public static float PlayerHeight = 2f; //人物高度

        public const string CHANGE_HP_MSG = "CHANGE_HP_MSG";
        public const string ENTER_GAME_MSG = "ENTER_GAME_MSG";
        public const string END_GAME_MSG = "END_GAME_MSG";
        public const string INIT_GAME_MSG = "INIT_GAME_MSG";
    }
}