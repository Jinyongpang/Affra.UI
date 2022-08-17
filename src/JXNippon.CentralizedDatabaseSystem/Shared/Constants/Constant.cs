using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Constants
{
    public static class Constant
    {
        public const string DialogStyle = "min-height:auto;max-width:100%; width:auto; min-width:30vw;";
        public const string MOHDialogStyle = "min-height:auto;min-width:80%;width:auto;top:100px;max-height:80%";

        public readonly static DialogOptions DialogOptions = new DialogOptions() { Style = DialogStyle, Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true };
        public readonly static DialogOptions MOHDialogOptions = new DialogOptions() { Style = MOHDialogStyle, Resizable = true, Draggable = false, CloseDialogOnOverlayClick = true };
    }
}
