using Radzen;

namespace JXNippon.CentralizedDatabaseSystem.Shared.Constants
{
    public static class Constant
    {
        public const string DialogStyle = "min-height:auto;min-width:600px;width:auto";

        public readonly static DialogOptions DialogOptions = new DialogOptions() { Style = DialogStyle, Resizable = true, Draggable = true, CloseDialogOnOverlayClick = true };
    }
}
