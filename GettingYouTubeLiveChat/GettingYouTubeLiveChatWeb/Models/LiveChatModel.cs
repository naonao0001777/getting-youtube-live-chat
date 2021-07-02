using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LiveChatModel
    {
        public string DspMessage { get; set; }

        public string DspName{get;set;}

        public string ChannelUrl { get; set; }

        public string ProfileImageUrl { get; set; }

        public DateTime? ChatDateTime { get; set; }
    }
}