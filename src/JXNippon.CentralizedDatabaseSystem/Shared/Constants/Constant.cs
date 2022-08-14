using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Constants
{
    public static class Constant
    {
        public const string DialogStyle = "min-height:auto;min-width:600px;width:auto";
        public const string MOHDialogStyle = "height:100vh;width:100vw;padding:0px";

        public readonly static DialogOptions DialogOptions = new DialogOptions() { Style = DialogStyle, Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true };
        public readonly static DialogOptions MOHDialogOptions = new DialogOptions() { Style = MOHDialogStyle, Resizable = true, Draggable = false, CloseDialogOnOverlayClick = true };
    }
}
