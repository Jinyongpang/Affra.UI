namespace JXNippon.CentralizedDatabaseSystem.Domain.CommonHelpers
{
    public class CommonHelper : ICommonHelper
    {
        public T Construct<T>()
        {
            return (T)Activator.CreateInstance(typeof(T));
        }
    }
}
