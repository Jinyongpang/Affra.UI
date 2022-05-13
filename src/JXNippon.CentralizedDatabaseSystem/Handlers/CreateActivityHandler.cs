using System.Diagnostics;

namespace JXNippon.CentralizedDatabaseSystem.Handlers
{
    public class CreateActivityHandler : DelegatingHandler
    {
        private readonly ILogger<CreateActivityHandler> _logger;

        public CreateActivityHandler(ILogger<CreateActivityHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            using Activity? activity = new Activity(nameof(SendAsync)).Start();
            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
