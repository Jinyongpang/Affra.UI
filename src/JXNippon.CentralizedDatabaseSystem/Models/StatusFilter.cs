namespace JXNippon.CentralizedDatabaseSystem.Models
{
    public static class StatusFilter
    {
        public static IEnumerable<string> Statuses = (new string[] { "Online", "Offline", "Standby", "NA", "Auto Standby", "Duty", "Hot Standby", "Schedule", "Downtime", "Shut-in", "Flowing", "In Service", "Maintenance" }).OrderBy(value => value);
    }
}
