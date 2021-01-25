using Newtonsoft.Json;
using SideKick.Examination.WS.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Commands
{
    public class LoginSaltCommand : BaseCommand
    {
        [JsonProperty("username")]
        public string UserName { get; set; }
    }

    public class LoginSaltResponse
    {
        [JsonProperty("command")]
        public string Command { get { return Cons.LoginSalt; } }

        [JsonProperty("username")]
        public string UserName { get; set; }

        [JsonProperty("validity")]
        public int Validity { get; set; }

        [JsonProperty("salt")]
        public string Salt { get; set; }
    }
}
