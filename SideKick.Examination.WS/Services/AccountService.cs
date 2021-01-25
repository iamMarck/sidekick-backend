using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SideKick.Examination.Data;
using SideKick.Examination.Data.Entities;
using SideKick.Examination.WS.Commands;
using SideKick.Examination.WS.Constants;
using SideKick.Examination.WS.Extensions;
using SideKick.Examination.WS.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Session;

namespace SideKick.Examination.WS.Services
{
    public class AccountService : BaseService<IAccountService>, IAccountService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        #region CONSTRUCTOR
        public AccountService(ILogger<IAccountService> logger,
            IHttpContextAccessor httpContextAccessor,
            ClientDbContext clientDbContext)
            :
            base(logger, clientDbContext)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        #endregion

        #region Implemented Functions
        /// <summary>
        /// Check if an email address is not yet registered in the system
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<CheckEmailResponse> CheckEmailAsync(CheckEmailCommand command)
        {
            var user = await this.ClientDbContext.Users.FirstOrDefaultAsync(f => f.Email == command.Email);

            return new CheckEmailResponse()
            {
                Available = user == null,
                Email = command.Email
            };
        }

        /// <summary>
        /// Check if a username is still available for registration
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<CheckUsernameResponse> CheckUserNameAsync(CheckUsernameCommand command)
        {
            var user = await this.ClientDbContext.Users.FirstOrDefaultAsync(f => f.UserName == command.Username);

            return new CheckUsernameResponse()
            {
                Available = user == null,
                Username = command.Username
            };
        }

        /// <summary>
        /// This command is used to create a new verificationCode and be sent to the email address provided
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<EmailVerificationResponse> EmailVerificationAsync(EmailVerificationCommand command)
        {
            var code = Extensions.SecurityExtension.RandomString(6);
            var email = command.Email;
            var suc = await EmailHelper.SendKeyCode(code, command.Email, command.UserName);
            if (suc && _httpContextAccessor.HttpContext.Items["code"] == null)
            {
                _httpContextAccessor.HttpContext.Items.Add($"em_{email}", code);
            }
            return new EmailVerificationResponse() { Success = suc };
        }

        /// <summary>
        /// Perform a login attempt.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<LoginResponse> LoginAsyncAsync(LoginCommand command)
        {
            var response = new LoginResponse() { UserName = command.UsernameOrEmail, Validity = Cons.SaltValidityINSeconds };

            var jdata = command;
            var user = await this.GetUserByEmailUserName(jdata.UsernameOrEmail, jdata.UsernameOrEmail);
            if (user != null)
            {
                var getUserSalt = user.UserSaltSessions.Where(w => w.DateCreated.AddSeconds(Cons.SaltValidityINSeconds) >= DateTime.UtcNow &&
                                        !w.Expired)?.OrderByDescending(o => o.DateCreated)?.FirstOrDefault()?.Salt;
                if (!string.IsNullOrWhiteSpace(getUserSalt))
                {
                    var validChallenge =  user.Password.jsSHA(getUserSalt);
                    if (jdata.Challenge == validChallenge)
                    {
                        response.SessionID = SecurityExtension.RandomString();
                        response.Success = true;
                    }
                }
            }
            return response;
        }

        /// <summary>
        /// This function is sent by the client to initiate a login attempt.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<LoginSaltResponse> LoginSaltAsync(LoginSaltCommand command)
        {
            var response = new LoginSaltResponse() { Salt = SecurityExtension.RandomString(), UserName = command.UserName, Validity = Cons.SaltValidityINSeconds };
            var user = await this.GetUserByEmailUserName(command.UserName, command.UserName);
            if (user != null)
            {
                var salt = response.Salt;
                var dtNow = DateTime.UtcNow;
                var valid = user.UserSaltSessions.Where(w => !w.Expired).OrderByDescending(o => o.DateCreated).ToList();

                if (valid.Count() == 0)
                {
                    AddUserSalt(user, salt);
                }
                else
                {
                    bool hasOld = false;
                    foreach (var ses in valid)
                    {
                        if (DateTime.UtcNow.Subtract(ses.DateCreated).TotalSeconds > Cons.SaltValidityINSeconds)
                            ses.Expired = true;
                        else
                        {
                            hasOld = true;
                            response.Salt = ses.Salt;
                        }
                    }
                    if(!hasOld) AddUserSalt(user, response.Salt);
                }

                await this.ClientDbContext.SaveChangesAsync();
            }
            return response;
        }

        /// <summary>
        /// Perform Registration
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public async Task<RegisterResponse> RegisterAsync(RegisterCommand command)
        {
            var response = new RegisterResponse() { UserName = command.Email };
            
            var dt = command;
            var email = dt.Email;
          
            if(_httpContextAccessor.HttpContext.Items[$"em_{email}"]?.ToString() ==  dt.VerificationCode)
            {
                this.ClientDbContext.Users.Add(new User { 
                    UserName = dt.Username,
                    Email = dt.Email,
                    DisplayName = dt.DisplayName,
                    Password = dt.Password.HashHmac(),
                });
                response.Success = await this.ClientDbContext.SaveChangesAsync() > 0;
            }
            return response;
        }


        #endregion

        #region PRIVATE Functions
        private void AddUserSalt(User user, string salt)
        {
            user.UserSaltSessions.Add(new UserSaltSession
            {
                Salt = salt,
                UserId = user.Id
            });
        }
        private async Task<User> GetUserByEmailUserName(string email, string username)
        {
            return await this.ClientDbContext.Users.Include(i=>i.UserSaltSessions).FirstOrDefaultAsync(f => f.UserName == username || f.Email == email);
        }
        #endregion
    }
}
