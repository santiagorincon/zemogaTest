using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.Models;

namespace ZemogaTechnicalTest.DTO
{
    public class PostDTO
    {
        public PostDTO()
        {

        }

        // This constructor is added for initialize an DTO object with a DB object
        public PostDTO(Post post)
        {
            ID = post.ID;
            PostName = post.PostName;
            PostContent = post.PostContent;
            AuthorID = post.AuthorID;
            AuthorName = post.Author != null ? post.Author.UserFullname : string.Empty;
            CreatedDate = post.CreatedDate;
            SubmitDate = post.SubmitDate;
            ApprovalDate = post.ApprovalDate;
            EditorID = post.EditorID;
            EditorName = post.Editor != null ? post.Editor.UserFullname : string.Empty;
            StatusID = post.StatusID;
            StatusName = post.Status != null ? post.Status.StatusName : string.Empty;
            Comments = post.PostComments.Any() ? post.PostComments.Select(p => new PostCommentDTO(p)).ToList() : new List<PostCommentDTO>();
        }

        //Attributes for PostDTO. AuthorName, EditorName and StatusName were added for return names of relationed items
        public int? ID { get; set; }
        public string PostName { get; set; }
        public string PostContent { get; set; }
        public int? AuthorID { get; set; }
        public string AuthorName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? SubmitDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public int? EditorID { get; set; }
        public string EditorName { get; set; }
        public int? StatusID { get; set; }
        public string StatusName { get; set; }
        public List<PostCommentDTO> Comments { get; set; }
    }
}
