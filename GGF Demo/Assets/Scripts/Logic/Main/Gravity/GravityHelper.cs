using Define;

namespace Logic
{
    public static class GravityHelper
    {
        public static float CalSpeedWithGravity(float speed,float time)
        {
            return speed -= GameDefine.MapGravity * time;
        }
    }
}