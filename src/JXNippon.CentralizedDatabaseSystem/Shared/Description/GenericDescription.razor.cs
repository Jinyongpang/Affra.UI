using Microsoft.AspNetCore.Components;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Description
{
    public partial class GenericDescription
    {
        [Parameter] public string Height { get; set; }
        [Parameter] public string Width { get; set; }
        [Parameter] public string Description { get; set; }
        [Parameter] public ImageFile ImageFile { get; set; }
        [Parameter] public string Class { get; set; }
        [Parameter] public RenderFragment? ChildContent { get; set; }

        private IDictionary<ImageFile, string> _images = new Dictionary<ImageFile, string>()
        {
            [ImageFile.AcceptTasks] = "img/accept_tasks.svg",
            [ImageFile.AllApproved] = "img/all_approved.svg",
            [ImageFile.CalendarAnalysis] = "img/calendar-analysis.svg",
            [ImageFile.ColumnConfig] = "img/column-config.svg",
            [ImageFile.Completed] = "img/completed.svg",
            [ImageFile.CompleteLogin] = "img/complete-login.svg",
            [ImageFile.DataExtraction] = "img/data_extraction.svg",
            [ImageFile.DatePicker] = "img/date-picker.svg",
            [ImageFile.Departing] = "img/departing.svg",
            [ImageFile.Designer] = "img/designer.svg",
            [ImageFile.ErrorForbidden] = "img/error_forbidden.svg",
            [ImageFile.ErrorNotFound] = "img/error_not_found.svg",
            [ImageFile.Gauge] = "img/gauge.svg",
            [ImageFile.LowBattery] = "img/low-battery.svg",
            [ImageFile.Merger] = "img/merger.svg",
            [ImageFile.NewTasks] = "img/new_task.svg",
            [ImageFile.NoData] = "img/no-data.svg",
            [ImageFile.Piechart] = "img/piechart.svg",
            [ImageFile.RangePicker] = "img/range-picker.svg",
            [ImageFile.Research] = "img/research.svg",
            [ImageFile.Search] = "img/search.svg",
            [ImageFile.Stats] = "img/stats.svg",
            [ImageFile.Steps] = "img/steps.svg",
            [ImageFile.Tasks] = "img/tasks.svg",
            [ImageFile.Teleport] = "img/teleport.svg",
            [ImageFile.TodoList] = "img/todo-list.svg",
            [ImageFile.WorkInProgress] = "img/work-in-progress.svg",
        };
    }
}
