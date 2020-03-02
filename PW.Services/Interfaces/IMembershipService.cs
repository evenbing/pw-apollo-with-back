using PW.DataTransferObjects.Users;
using PW.Entities;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PW.Services.Interfaces
{
    public interface IMembershipService
    {
        ClaimsPrincipal GetUserClaimsPrincipal(UserDto user);
        Task<UserDto> CreateUserAsync(SignUpDto signUpDto);
        Task<UserDto> GetUserAsync(LoginDto loginDto);
    }
}
