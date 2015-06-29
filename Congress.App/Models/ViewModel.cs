using Congress.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Congress.App.Models
{
    public class ViewModel
    {
        public string UID { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}