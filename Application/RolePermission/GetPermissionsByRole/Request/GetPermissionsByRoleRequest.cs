﻿
using Domain.Enum;

namespace Application.RolePermission.GetPermissionsByRole.Request
{
    public class GetPermissionsByRoleRequest : RolePermission.RequestRolePermission
    {
        //FIXME this should be enum
        public AccountRole Role { get; set; }
    }
}
