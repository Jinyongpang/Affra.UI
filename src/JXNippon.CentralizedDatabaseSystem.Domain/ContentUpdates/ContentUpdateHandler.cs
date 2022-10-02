namespace JXNippon.CentralizedDatabaseSystem.Domain.ContentUpdates
{
    public class ContentUpdateHandler
    {
        public ContentUpdateHandler()
        {
        }

        public Func<object, Task> Handler { get; set; }

        public Task OnContentUpdateAsync(object data)
        {
            return Handler(data);
        }
    }
}
