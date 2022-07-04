using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace JXNippon.CentralizedDatabaseSystem.Shared
{
    public partial class GenericProgressBar
    {
        [Parameter] public int Percentage { get; set; }
        [Parameter] public int StrokeWidth { get; set; }
        [Parameter] public bool ShowInfo { get; set; }

        private string GetColorCode()
        {
            string colorCode = "#17A2B8";

            switch (Percentage)
            {
                case <= 20:
                    colorCode = "#FF0000";
                    break;
                case <= 40:
                    colorCode = "#F3880B";
                    break;
                case <= 60:
                    colorCode = "#FFC107";
                    break;
                case <= 80:
                    colorCode = "#007BFF";
                    break;
                case <= 100:
                    colorCode = "#00FF00";
                    break;
                default:
                    colorCode = "#17A2B8";
                    break;
            }

            return colorCode;
        }
        private string GetColorClass()
        {
            string colorClass = "progressBarTitle";

            switch (Percentage)
            {
                case <= 20:
                    colorClass = "progressBarTitle pb-red";
                    break;
                case <= 40:
                    colorClass = "progressBarTitle pb-orange";
                    break;
                case <= 60:
                    colorClass = "progressBarTitle pb-yellow";
                    break;
                case <= 80:
                    colorClass = "progressBarTitle pb-blue";
                    break;
                case <= 100:
                    colorClass = "progressBarTitle pb-green";
                    break;
                default:
                    colorClass = "progressBarTitle";
                    break;
            }

            return colorClass;
        }
    }
}
