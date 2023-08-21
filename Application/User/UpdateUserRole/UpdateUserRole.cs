﻿using Application.Enums;
using Application.Ports;
using Application.User.SignOut.Response;
using Application.User.UpdateUserRole.Request;
using Application.User.UpdateUserRole.Response;
using Domain.Enum;
using Microsoft.Extensions.Logging;

namespace Application.User.UpdateUserRole
{
    public class UpdateUserRole : IUpdateUserRole
    {
        private readonly ILogger _logger;
        private readonly IAuthRepository _authRepository;
        public UpdateUserRole(ILogger<UpdateUserRole> logger,
            IAuthRepository authRepository)
        {
            _logger = logger;
            _authRepository = authRepository;
        }
        //FIXME refactor method and sparate error handling
        public async Task<UpdateUserRoleResponse> Execute(UpdateUserRoleRequest request)
        {
            try
            {
                var userExists = await _authRepository.CheckIfUserExists(request.UserId);
                if (userExists == false)
                {
                    return new UpdateUserRoleErrorResponse
                    {
                        Message = Enum.GetName(ErrorCodes.UserDoesNotExist),
                        Code = ErrorCodes.UserDoesNotExist.ToString("D")
                    };
                }
                if (Enum.TryParse(request.AccountRole, out AccountRole parsedRole))
                {
                    await _authRepository.UpdateUserRole(request.UserId, parsedRole);
                    return new UpdateUserRoleSuccessResponse
                    {
                        Message = "User role updated."
                    };
                }
                else
                {
                    return new UpdateUserRoleErrorResponse
                    {
                        Message = Enum.GetName(ErrorCodes.AccountRoleIsNotCorrect),
                        Code = ErrorCodes.AccountRoleIsNotCorrect.ToString("D")
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new UpdateUserRoleErrorResponse
                {
                    Code = ErrorCodes.AnUnexpectedErrorOcurred.ToString("D"),
                    Message = Enum.GetName(ErrorCodes.AnUnexpectedErrorOcurred)
                };
            }
        }
    }
}
