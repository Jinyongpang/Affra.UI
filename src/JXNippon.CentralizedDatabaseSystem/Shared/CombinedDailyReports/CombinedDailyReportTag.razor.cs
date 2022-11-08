using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.CombinedDailyReports
{
    public partial class CombinedDailyReportTag
    {
        [Parameter] public int[] Counts { get; set; }
        public bool HasNoViolation { get; set; }

        public bool HasEmptyRowData { get; set; }

        public int TotalCount { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        private int GetTotalCount()
        {
            this.TotalCount = 0;
            this.HasEmptyRowData = false;
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

            return this.TotalCount;

        }

        private string GetColor()
        {
            return this.HasNoViolation
                ? "Green"
                : "Red";
        }



    }
}
