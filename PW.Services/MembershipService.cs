using AutoMapper;
using Microsoft.Extensions.Configuration;
using PW.DataAccess.Interfaces;
using PW.DataTransferObjects.Users;
using PW.Entities;
using PW.Services.Exceptions;
using PW.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PW.Services
{
    public class MembershipService : IMembershipService
    {
        private const string InvalidUsernameOrPasswordMessage = "Invalid username or password";
        private const string EmailIsAlreadyRegistredMessage = "Email \"{0}\" is already registred";
        private const string UsernameIsAlreadyRegistredMessage = "Username \"{0}\" is already registred";
        private const string CanNotGetStartBalanceMessage = "Can not get start balance from config";

        private readonly IConfiguration _configuration;
        private readonly IUserRepository _userRepository;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;

        public MembershipService(IConfiguration configuration, IUserRepository userRepository, IEncryptionService encryptionService, IMapper mapper)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _mapper = mapper;
        }

        public ClaimsPrincipal GetUserClaimsPrincipal(UserDto userDto)
        {
            ClaimsPrincipal claimsPrincipal = null;
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userDto.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
            claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            return claimsPrincipal;
        }

        public async Task<UserDto> GetUserAsync(LoginDto loginDto)
        {
            var user = await _userRepository.GetByEmailAsync(loginDto.Email);
            if (user == null || !IsUserValid(user, loginDto.Password))
            {
                throw new PWException(InvalidUsernameOrPasswordMessage);
            }
            var result = _mapper.Map<UserDto>(user);
            return result;
        }

        public async Task<UserDto> CreateUserAsync(SignUpDto signUpDto)
        {
            await CheckUserRegistred(signUpDto);

            var passwordSalt = _encryptionService.CreateSalt();
            int startBalance;
            try
            {
                startBalance = int.Parse(_configuration.GetSection("User:StartBalance").Value);
            }
            catch (Exception)
            {
                throw new PWException(CanNotGetStartBalanceMessage);
            }

            var user = new PwUser()
            {
                UserName = signUpDto.UserName,
                Salt = passwordSalt,
                Email = signUpDto.Email,
                PasswordHash = _encryptionService.EncryptPassword(signUpDto.Password, passwordSalt),
                Balance = startBalance
            };

            await _userRepository.AddAsync(user);
            return _mapper.Map<UserDto>(user);
        }        

        private bool IsUserValid(PwUser user, string password)
        {
            var result = string.Equals(_encryptionService.EncryptPassword(password, user.Salt), user.PasswordHash);
            return result;
        }

        private async Task CheckUserRegistred(SignUpDto signUpDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(signUpDto.Email);
            if (existingUser != null)
            {
                throw new PWException(string.Format(EmailIsAlreadyRegistredMessage, signUpDto.Email));
            }
            existingUser = await _userRepository.GetByNameAsync(signUpDto.UserName);
            if (existingUser != null)
            {
                throw new PWException(string.Format(UsernameIsAlreadyRegistredMessage, signUpDto.UserName));
            }
        }
    }
}
