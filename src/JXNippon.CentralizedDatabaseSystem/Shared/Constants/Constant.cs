using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Constants
{
    public static class Constant
    {
        public const string DialogStyle = "min-height:auto;max-width:100%; width:auto; min-width:30vw;";
        public const string MOHDialogStyle = "height:100vh;width:100vw;padding:0px";

        public readonly static DialogOptions DialogOptions = new DialogOptions() { Style = DialogStyle, Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true };
        public readonly static DialogOptions MOHDialogOptions = new DialogOptions() { Style = MOHDialogStyle, Resizable = true, Draggable = false, CloseDialogOnOverlayClick = true };

        public readonly static string[] Colors = new string[]
        {
            "#3AB4F2",
            "#3BACB6",
            "#F9CEEE",
            "#F32424",
            "#E8630A",
            "#E15FED",
            "#F2FA5A",
            "#FFC0D3",
            "#7A0BC0",
            "#FED1EF",
            "#93FFD8",
            "#C7B198",
            "#064635",
            "#FF5403",
            "#009DAE",
            "#9D5C0D",
            "#AA14F0",
            "#B91646",
            "#49FF00",
            "#39A388",
            "#3DB2FF",
            "#0CECDD",
            "#FF94CC",
            "#2F5D62",
            "#1F441E",
        };

        public static string[] GetRandomColors()
        {
            return Colors
                .OrderBy(c => new Random().Next())
                .ToArray();
        }
    }
}
