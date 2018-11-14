using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ZumperFeed.Models
{
    public class FeedData
    {
        public int PropertyID { get; set; }
        public List<int> Units { get; set; } = null;
    }
}