using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest.Models
{
    public class PostComment
    {
        public int ID { get; set; }
        [ForeignKey("User")]
        public int? UserID { get; set; }
        public User User { get; set; }
        [ForeignKey("Post")]
        public int PostID { get; set; }
        public Post Post { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
