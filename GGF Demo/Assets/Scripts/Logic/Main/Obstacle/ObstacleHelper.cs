namespace Logic
{
    public static class ObstacleHelper
    {
        private static int obstacleIndex = 0;
        public static int GetObstacleIndex()
        {
            return obstacleIndex++;
        }
    }
}