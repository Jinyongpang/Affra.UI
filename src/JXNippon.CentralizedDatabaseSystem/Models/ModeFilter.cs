namespace JXNippon.CentralizedDatabaseSystem.Models
{
    public static class ModeFilter
    {
        public static IEnumerable<string> Modes = (new string[] { "Manual", "Auto" }).OrderBy(value => value);
    }
}
