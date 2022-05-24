namespace TeamZ.Code.Helpers
{
    public class Singletone<TInstance>
        where TInstance : new()
    {
        static Singletone()
        {
            Instance = new TInstance();
        }

        public static TInstance Instance
        {
            get;
        }
    }
}
