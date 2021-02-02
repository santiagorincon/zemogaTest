using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest_FRONT.DTO
{
    public class PostCommentDTO
    {
        public int? UserID { get; set; }
        public string UserFullname { get; set; }
        public int PostID { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
