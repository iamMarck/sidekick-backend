using Newtonsoft.Json;
using SideKick.Examination.WS.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Commands
{
    public class CheckEmailCommand : BaseCommand
    {
        [JsonProperty("email")]
        public string Email { get; set; }
    }

    public class CheckEmailResponse
    {
        [JsonProperty("command")]
        public string Command { get { return Cons.CheckEmail; } }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("available")]
        public bool Available { get; set; }

    }
}
