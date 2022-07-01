namespace JXNippon.CentralizedDatabaseSystem.Domain.Hubs
{
    public interface IHubClient<T> where T : HubClientConfigurationsBase
    {
        IHubSubscription Subscribe<T1>(ICollection<string> methodNames, Func<T1, Task> handler, CancellationToken cancellationToken = default);
    }
}
