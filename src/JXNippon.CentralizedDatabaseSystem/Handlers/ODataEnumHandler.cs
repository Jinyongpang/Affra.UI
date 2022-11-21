namespace JXNippon.CentralizedDatabaseSystem.Handlers
{
    public class ODataEnumHandler : DelegatingHandler
    {
        private readonly ILogger<ODataEnumHandler> _logger;

        public ODataEnumHandler(ILogger<ODataEnumHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            request.RequestUri = new Uri(request.RequestUri.ToString().Replace("/System.ToString()", ""));
            var response = await base.SendAsync(request, cancellationToken);

            return response;
        }
    }
}
