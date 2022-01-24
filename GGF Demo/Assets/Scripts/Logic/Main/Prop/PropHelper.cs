namespace Logic
{
    public static class PropHelper
    {
        private static int propIndex = 0;
        public static int GetPropIndex()
        {
            return propIndex++;
        }
    }
}