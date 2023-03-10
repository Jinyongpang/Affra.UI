using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.PEMonthlyReports
{
    public partial class PEMonthlyReportTag
    {
        [Parameter] public int[] Counts { get; set; }

        public bool HasNoViolation { get; set; }

        public bool HasEmptyRowData { get; set; }

        public int TotalCount { get; set; }

        protected override void OnInitialized()
        {
            this.TotalCount = 0;
            foreach (var item in Counts)
            {
                if (item >= 0)
                {
                    this.TotalCount += item;
                }
                else
                {
                    HasEmptyRowData = true;
                }
            }
            HasNoViolation = !this.HasEmptyRowData
                && this.TotalCount == 0;

            base.OnInitialized();
        }

        private string GetColor()
        {
            return this.HasNoViolation
                ? "Green"
                : "Red";
        }



    }
}
