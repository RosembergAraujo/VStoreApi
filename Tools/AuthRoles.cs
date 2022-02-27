namespace VStoreAPI.Tools
{
    public static class AuthRoles
    {
        public static string[] Roles { get; } = { "admin", "dev", "client" };
        public static string[] HighPrivilegesRoles { get; } = { "admin", "dev" };
        public static string[] LowPrivilegesRoles { get; } = { "client" };
    }
}