using System.Linq;
using System.Security.Principal;

namespace VStoreAPI.Tools
{
    public static class AuthRoles
    {
        public static string[] Roles { get; } = { "admin", "dev", "client", "vendor" };
        public static string[] HighPrivilegesRoles { get; } = { "admin", "dev" };
        public static string[] LowPrivilegesRoles { get; } = { "client", "vendor" };

        public static bool IsUserWithHighPrivileges(IPrincipal claim)
            => HighPrivilegesRoles.Any(claim.IsInRole);

        public static bool IsUserWithLowPrivileges(IPrincipal claim)
            => LowPrivilegesRoles.Any(claim.IsInRole);

        public static bool IsUserInRoles(IPrincipal claim)
            => Roles.Any(claim.IsInRole);
        
    }

    
}