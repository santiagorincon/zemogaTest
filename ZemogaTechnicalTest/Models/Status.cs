using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest.Models
{
    public class Status
    {
        public int ID { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string StatusDesc { get; set; }

        public List<Post> Posts { get; set; }
        public List<PostActivity> OldStatusActivities { get; set; }
        public List<PostActivity> NewStatusActivities { get; set; }
    }
}
