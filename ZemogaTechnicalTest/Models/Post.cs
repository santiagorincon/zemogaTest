using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest.Models
{
    public class Post
    {
        public Post()
        {
            PostActivities = new List<PostActivity>();
            PostComments = new List<PostComment>();
        }
        public int ID { get; set; }
        public string PostName { get; set; }
        public string PostContent { get; set; }
        [ForeignKey("Author")]
        public int AuthorID { get; set; }
        public User Author { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        [ForeignKey("Editor")]
        public int? EditorID { get; set; }
        public User Editor { get; set; }
        [ForeignKey("Status")]
        public int StatusID { get; set; }
        public Status Status { get; set; }

        public List<PostActivity> PostActivities { get; set; }
        public List<PostComment> PostComments { get; set; }
    }
}
