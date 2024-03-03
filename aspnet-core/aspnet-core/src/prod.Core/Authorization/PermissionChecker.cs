using Abp.Authorization;
using prod.Authorization.Roles;
using prod.Authorization.Users;

namespace prod.Authorization
{
    public class PermissionChecker : PermissionChecker<Role, User>
    {
        public PermissionChecker(UserManager userManager)
            : base(userManager)
        {

        }
    }
}
