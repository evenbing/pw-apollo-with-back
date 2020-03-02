using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PW.DataAccess.Interfaces;
using PW.DataTransferObjects.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PW.Web.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;            
        }
                
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetUsernameOptions()
        {
            var email = HttpContext.User.Identity.Name;
            var users = await _userRepository.FindByAsync(x => x.Email != email);
            var result = users.Select(u => u.UserName);
            return Ok(result);
        }
    }
}
