
namespace CoreFramework
{
    public static class IDGenerator
    {
        private static int m_ID;

        public static int GenerateID()
        {
            return m_ID++;
        }
    }
}