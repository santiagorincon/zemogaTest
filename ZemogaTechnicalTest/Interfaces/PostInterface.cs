using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.DTO;

namespace ZemogaTechnicalTest.Interfaces
{
    public interface PostInterface
    {
        //In this interface are defined the methods that will be implemented by the repository
        PostDTO UpdatePostContent(PostDTO content);
        PostDTO UpdatePostStatus(int postId, int statusId, int userId);
        List<PostDTO> GetPostsByStatus(int statusId);
        List<PostDTO> GetPostsByUserId(int userId);
        PostDTO GetPostById(int postId);
        PostDTO CommentPost(PostCommentDTO comment);
    }
}
