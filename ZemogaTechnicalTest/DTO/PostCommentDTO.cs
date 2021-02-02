using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.Models;

namespace ZemogaTechnicalTest.DTO
{
    public class PostCommentDTO
    {
        public PostCommentDTO()
        {

        }

        public PostCommentDTO(PostComment comment)
        {
            UserID = comment.UserID;
            UserFullname = comment.User != null ? comment.User.UserFullname : "Anonymous User";
            PostID = comment.PostID;
            Comment = comment.Comment;
            CreatedDate = comment.CreatedDate;
        }
        public int? UserID { get; set; }
        public string UserFullname { get; set; }
        public int PostID { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
