using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Constants
{
    public static class Constant
    {
        public const string DialogStyle = "min-height:auto;max-width:100%; width:auto; min-width:30vw;";
        public const string MOHDialogStyle = "height:100vh;width:100vw;padding:0px; border-radius:0 !important;";

        public readonly static DialogOptions DialogOptions = new DialogOptions() { Style = DialogStyle, Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true };
        public readonly static DialogOptions MOHDialogOptions = new DialogOptions() { Style = MOHDialogStyle, Resizable = false, Draggable = false, ShowClose = false, ShowTitle = false, CloseDialogOnOverlayClick = true };

        public readonly static string[] Colors = new string[]
        {
            "#850E35",
            "#42855B",
            "#FF9551",
            "#A62349",
            "#319DA0",
            "#277BC0",
            "#AC4425",
            "#2B4865",
            "#80558C",
            "#A47E3B",
            "#7F5283",
            "#66BFBF",
            "#7858A6",
            "#7D1E6A",
            "#0AA1DD",
            "#AC7D88",
            "#005555",
            "#FFD124",
            "#0E185F",
            "#5463FF",
            "#789395",
            "#FC28FB",
            "#FFAB76",
            "#406882",
            "#9A0680",
        };

        public static string[] GetRandomColors()
        {
            return Colors
                .OrderBy(c => new Random().Next())
                .ToArray();
        }
    }
}
