using Newtonsoft.Json;
using SideKick.Examination.WS.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Commands
{
    public class EmailVerificationCommand : BaseCommand
    {
        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("username")]
        public string UserName { get; set; }
    }

    public class EmailVerificationResponse
    {
        [JsonProperty("command")]
        public string Command { get { return Cons.EmailVerification; } }

        [JsonProperty("success")]
        public bool Success { get; set; }

    }
}
