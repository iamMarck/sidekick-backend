using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SideKick.Examination.WS.Commands
{
    public class JData
    {
        [JsonProperty("jsonData")]
        public string JsonData { get; set; }
    }
    public class JsonCommand<T> 
    {
        [JsonProperty("jsonData")]
        public T JsonData { get; set; }



    }

    public class BaseCommand
    {
        [JsonProperty("command")]
        public string Command { get; set; }
    }
}
