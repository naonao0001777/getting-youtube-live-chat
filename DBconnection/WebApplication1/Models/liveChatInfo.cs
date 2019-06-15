using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class liveChatInfo
    {
        public string dspMessage { get; set; }

        public string dspName { get; set; }

        public List<string> dspMessagesList { get; set; }

        public List<string> dspNamesList { get; set; }
    }
}