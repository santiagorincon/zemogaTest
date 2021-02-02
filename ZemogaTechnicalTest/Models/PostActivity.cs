using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest.Models
{
    public class PostActivity
    {
        public int ID { get; set; }
        [ForeignKey("Post")]
        public int PostID { get; set; }
        public Post Post { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public User User { get; set; }
        [ForeignKey("OldStatus")]
        public int? OldStatusID { get; set; }
        public Status OldStatus { get; set; }
        [ForeignKey("NewStatus")]
        public int? NewStatusID { get; set; }
        public Status NewStatus { get; set; }
        public DateTime ActivityDate { get; set; }
    }
}
