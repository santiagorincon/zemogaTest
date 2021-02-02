using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.Data;
using ZemogaTechnicalTest.DTO;
using ZemogaTechnicalTest.Interfaces;
using ZemogaTechnicalTest.Models;
using ZemogaTechnicalTest.Tools;

namespace ZemogaTechnicalTest.Repositories
{
    public class PostRepository : PostInterface
    {
        private readonly ZemogaContext _dbContext;
        private static int _initialStatus;
        private static int _deletedStatus;
        private static int _approvedStatus;
        private static int _pendingStatus;
        public PostRepository(ZemogaContext dbContext, GlobalVariables globalVariables)
        {
            _dbContext = dbContext;
            _initialStatus = globalVariables.InitialStatus;
            _deletedStatus = globalVariables.DeletedStatus;
            _approvedStatus = globalVariables.PublishedStatus;
            _pendingStatus = globalVariables.PendingStatus;
        }

        public PostDTO GetPostById(int postId)
        {
            PostDTO post = _dbContext.Posts
                .Include(p => p.Author)
                .Include(p => p.Editor)
                .Include(p => p.Status)
                .Include(p => p.PostComments)
                .Where(p => p.ID == postId)
                .Select(p => new PostDTO(p)).FirstOrDefault();

            if (post == null)
            {
                throw new Exception(string.Format("Post not found: {0}", postId));
            }

            return post;
        }

        public List<PostDTO> GetPostsByStatus(int statusId)
        {
            if (_dbContext.Status.Find(statusId) == null)
            {
                throw new Exception(string.Format("Status not found: {0}", statusId));
            }

            return _dbContext.Posts
                .Include(p => p.Author)
                .Include(p => p.Editor)
                .Include(p => p.Status)
                .Include(p => p.PostComments)
                .Where(p => p.StatusID == statusId && p.StatusID != _deletedStatus)
                .Select(p => new PostDTO(p)).ToList();
        }

        public List<PostDTO> GetPostsByUserId(int userId)
        {
            if (_dbContext.Users.Find(userId) == null)
            {
                throw new Exception(string.Format("User not found: {0}", userId));
            }

            return _dbContext.Posts
                .Include(p => p.Author)
                .Include(p => p.Editor)
                .Include(p => p.Status)
                .Include(p => p.PostComments)
                .Where(p => p.AuthorID == userId && p.StatusID != _deletedStatus)
                .Select(p => new PostDTO(p)).ToList();
        }

        public PostDTO UpdatePostContent(PostDTO content)
        {
            Post post = null;
            int? _oldStatus = null;
            int? _newStatus = _initialStatus;
            if (content.ID == null)
            {
                post = new Post
                {
                    AuthorID = (int)content.AuthorID
                };

                _dbContext.Posts.Add(post);
            }
            else
            {
                post = _dbContext.Posts.Find((int)content.ID);
                if(post == null)
                {
                    throw new Exception(string.Format("Post not found: {0}", content.ID));
                }
                _dbContext.Entry(post).State = EntityState.Modified;
                _oldStatus = post.StatusID;
            }

            post.PostName = content.PostName;
            post.PostContent = content.PostContent;
            post.StatusID = _initialStatus;

            post.PostActivities.Add(new PostActivity
            {
                OldStatusID = _oldStatus,
                NewStatusID = _newStatus,
                UserID = (int)content.AuthorID
            });

            _dbContext.SaveChanges();

            return GetPostById(post.ID);
        }

        public PostDTO UpdatePostStatus(int postId, int statusId, int userId)
        {
            Post post = _dbContext.Posts.Find(postId);
            if (post == null)
            {
                throw new Exception(string.Format("Post not found: {0}", postId));
            }
            _dbContext.Entry(post).State = EntityState.Modified;

            post.PostActivities.Add(new PostActivity
            {
                OldStatusID = post.StatusID,
                NewStatusID = statusId,
                UserID = userId
            });

            post.StatusID = statusId;
            if(statusId == _approvedStatus)
            {
                post.ApprovalDate = DateTime.Now;
                post.EditorID = userId;
            }
            else if (statusId == _pendingStatus)
            {
                post.SubmitDate = DateTime.Now;
            }
            else
            {
                post.EditorID = userId;
            }

            _dbContext.SaveChanges();
            return new PostDTO(post);
        }

        public PostDTO CommentPost(PostCommentDTO comment)
        {
            Post post = _dbContext.Posts.Find(comment.PostID);
            if (post == null)
            {
                throw new Exception(string.Format("Post not found: {0}", comment.PostID));
            }
            _dbContext.Entry(post).State = EntityState.Modified;

            post.PostComments.Add(new PostComment
            {
                UserID = comment.UserID,
                Comment = comment.Comment
            });

            _dbContext.SaveChanges();
            return new PostDTO(post);
        }
    }
}
