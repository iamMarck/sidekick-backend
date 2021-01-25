using Newtonsoft.Json;
using SideKick.Examination.WS.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Commands
{
    public class LoginCommand : BaseCommand
    {
        [JsonProperty("usernameOrEmail")]
        public string UsernameOrEmail { get; set; }

        [JsonProperty("challenge")]
        public string Challenge { get; set; }
    }

    public class LoginResponse
    {
        [JsonProperty("command")]
        public string Command { get { return Cons.Login; } }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("success")]
        public bool Success { get; set; }

        [JsonProperty("sessionID")]
        public string SessionID { get; set; }

        [JsonProperty("validity")]
        public int Validity { get; set; }
    }
}
