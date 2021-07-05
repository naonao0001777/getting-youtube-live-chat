using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class LiveChatModelList
    {
        public List<LiveChatModel> ChatList { get; set; }

        public List<LiveChatModel> ChildList { get; set; }
    }
}