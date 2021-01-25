using Newtonsoft.Json;
using SideKick.Examination.WS.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Commands
{
    public class RegisterCommand : BaseCommand
    {
        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("password2")]
        public string Password2 { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("verificationCode")]
        public string VerificationCode { get; set; }
    }

    public class RegisterResponse
    {
        [JsonProperty("command")]
        public string Command { get { return Cons.Register; } }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

    }
}
