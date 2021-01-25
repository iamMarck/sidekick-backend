using Newtonsoft.Json;
using SideKick.Examination.WS.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Commands
{
    public class CheckUsernameCommand : BaseCommand
    {
        [JsonProperty("username")]
        public string Username { get; set; }
    }

    public class CheckUsernameResponse
    {
        [JsonProperty("command")]
        public string Command { get { return Cons.CheckUsername; } }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }

    }
}
