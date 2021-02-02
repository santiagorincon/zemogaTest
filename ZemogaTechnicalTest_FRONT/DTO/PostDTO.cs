using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest_FRONT.DTO
{
    public class PostDTO
    {
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
