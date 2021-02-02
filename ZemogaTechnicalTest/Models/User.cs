using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest.Models
{
    public class User
    {
        public int ID { get; set; }
        public string UserFullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [ForeignKey("Role")]
        public int RoleID { get; set; }
        public Role Role { get; set; }
        public DateTime CreatedDate { get; set; }

        public List<Post> CreatedPosts { get; set; }
        public List<Post> EditedPosts { get; set; }
        public List<PostActivity> PostActivities { get; set; }
        public List<PostComment> PostComments { get; set; }
    }
}
