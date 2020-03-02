using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.DataAccess.Interfaces;
using PW.DataTransferObjects.Users;
using PW.Entities;
using PW.Services.Exceptions;
using PW.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace PW.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SessionController : ControllerBase
    {
        private const string InvalidUserDataMessage = "Invalid user data";
        private const string CurrentUserNotFoundMessage = "Current user not found";

        private readonly IMembershipService _membershipService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public SessionController(IMembershipService membershipService,
            IUserRepository userRepository,
            IMapper mapper)
        {
            _membershipService = membershipService;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<ActionResult<UserBalanceDto>> Login([FromBody] LoginDto loginDto)
        {
            UserDto userDto;
            ClaimsPrincipal claimsPrincipal;            
            try
            {
                userDto = await _membershipService.GetUserAsync(loginDto);
                claimsPrincipal = _membershipService.GetUserClaimsPrincipal(userDto);
            }
            catch (PWException ex)
            {
                return BadRequest(new { errorMessage = ex.Message});
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            return Ok(userDto);
        }
                
        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Ok();
        }
                
        [HttpPost]
        public async Task<ActionResult<UserBalanceDto>> SignUp([FromBody] SignUpDto signUpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(InvalidUserDataMessage);
            }

            UserDto userDto;
            ClaimsPrincipal claimsPrincipal;
            try
            {
                userDto = await _membershipService.CreateUserAsync(signUpDto);
                claimsPrincipal = _membershipService.GetUserClaimsPrincipal(userDto);
            }
            catch (PWException ex)
            {
                return BadRequest(new { errorMessage = ex.Message });
            }

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal);

            var result = _mapper.Map<UserBalanceDto>(userDto);
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserBalanceDto>> GetSessionInfo()
        {
            var email = HttpContext.User.Identity.Name;
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                return BadRequest(new { errorMessage = CurrentUserNotFoundMessage });
            }

            var result = _mapper.Map<UserBalanceDto>(user);
            return Ok(result);
        }
    }
}
