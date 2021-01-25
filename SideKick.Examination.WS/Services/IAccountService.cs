using SideKick.Examination.WS.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Services
{
    public interface IAccountService
    {
        public Task<CheckEmailResponse> CheckEmailAsync(CheckEmailCommand command);

        public Task<CheckUsernameResponse> CheckUserNameAsync(CheckUsernameCommand command);

        public Task<EmailVerificationResponse> EmailVerificationAsync(EmailVerificationCommand command);

        public Task<LoginResponse> LoginAsyncAsync(LoginCommand command);

        public Task<LoginSaltResponse> LoginSaltAsync(LoginSaltCommand command);

        public Task<RegisterResponse> RegisterAsync(RegisterCommand command);

    }
}
