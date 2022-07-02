namespace JXNippon.CentralizedDatabaseSystem.Pages
{
    public partial class Home : IDisposable
    {
        private bool isDisposed = false;

        protected override async Task OnInitializedAsync()
        {
        }

        void IDisposable.Dispose()
        {
            if (!isDisposed)
            {
                isDisposed = true;
            }
        }
    }
}
