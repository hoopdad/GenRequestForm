using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.Graph.CoreConstants;

namespace ChatGPTRunner.Models
{
    public class Bloggerv3
    {
        public string? BlogID { get; set; }
        public string? Url { get; set; }
        //= "https://www.googleapis.com/blogger/v3/blogs/{BLOG_ID}/posts/";
        public string Kind { get; set; } = "blogger#post";
        public string? Title { get; set; }
        public string? Content { get; set; }
    }
}
