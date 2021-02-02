using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZemogaTechnicalTest.DTO;
using ZemogaTechnicalTest.Interfaces;
using ZemogaTechnicalTest.Tools;

namespace ZemogaTechnicalTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostInterface _postRepository;
        private readonly AuthenticationInterface _authRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private static int _initialStatus;
        private static int _pendingStatus;
        private static int _publishedStatus;
        private static int _rejectedStatus;
        private static int _deletedStatus;
        private static int _writerRole;
        private static int _editorRole;
        private int? userId = null;
        private int? roleId = null;

        public PostController(PostInterface postRepository, AuthenticationInterface authRepository, GlobalVariables globalVariables, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _postRepository = postRepository;
            _authRepository = authRepository;
            _initialStatus = globalVariables.InitialStatus;
            _pendingStatus = globalVariables.PendingStatus;
            _publishedStatus = globalVariables.PublishedStatus;
            _rejectedStatus = globalVariables.RejectedStatus;
            _deletedStatus = globalVariables.DeletedStatus;
            _writerRole = globalVariables.WriterRole;
            _editorRole = globalVariables.EditorRole;

            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;

            string userEncrypted = ((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).Claims.FirstOrDefault(s => s.Type == "userid")?.Value;
            if(userEncrypted != null)
            {
                userId = Int32.Parse(Encryptor.Decrypt(userEncrypted, _configuration["privateKey"]));
                roleId = Int32.Parse(((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).Claims.FirstOrDefault(s => s.Type == "userrole")?.Value);
            }
        }

        [HttpGet]
        public IActionResult Get()
        {
            var publishedPost = _postRepository.GetPostsByStatus(_publishedStatus);
            return new OkObjectResult(publishedPost);
        }

        [HttpGet("{id}", Name = "GetPostById")]
        public IActionResult GetPostById(int id)
        {
            var post = _postRepository.GetPostById(id);
            return new OkObjectResult(post);
        }

        [HttpGet("own", Name = "GetOwnPosts")]
        [Authorize]
        public IActionResult GetOwnPosts()
        {
            if(roleId != _writerRole)
            {
                throw new Exception("You're not a writer user, this action is only for writer users");
            }
            var ownPosts = _postRepository.GetPostsByUserId((int)userId);
            return Ok(new DefaultResponseDTO
            {
                token = _authRepository.RefreshToken(),
                response = ownPosts
            });
        }

        [HttpGet("pending", Name = "GetPendingPosts")]
        [Authorize]
        public IActionResult GetPendingPosts()
        {
            if (roleId != _editorRole)
            {
                throw new Exception("You're not a editor user, this action is only for editor users");
            }
            var pendingPost = _postRepository.GetPostsByStatus(_pendingStatus);
            return Ok(new DefaultResponseDTO
            {
                token = _authRepository.RefreshToken(),
                response = pendingPost
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] PostDTO post)
        {
            if (roleId != _writerRole)
            {
                throw new Exception("You're not a writer user, this action is only for writer users");
            }

            if (post.ID != null)
            {
                throw new Exception("Post ID is not null, please edit in the PUT service");
            }
            post.AuthorID = userId;
            using (var scope = new TransactionScope())
            {
                post = _postRepository.UpdatePostContent(post);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = post.ID }, post);
            }
        }

        [HttpPut]
        [Authorize]
        public IActionResult Put([FromBody] PostDTO post)
        {
            if (roleId != _writerRole)
            {
                throw new Exception("You're not a writer user, this action is only for writer users");
            }

            if (post.ID == null)
            {
                throw new Exception("Post ID is null, please create in the POST service");
            }

            post.AuthorID = userId;
            PostDTO oldPost = _postRepository.GetPostById((int)post.ID);

            if(oldPost.StatusID != _initialStatus && oldPost.StatusID != _rejectedStatus)
            {
                throw new Exception("Post cannot be edited");
            }

            using (var scope = new TransactionScope())
            {
                post = _postRepository.UpdatePostContent(post);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = post.ID }, post);
            }
        }

        [HttpPut("submit/{id}", Name = "SubmitPost")]
        [Authorize]
        public IActionResult SubmitPost(int id)
        {
            if (roleId != _writerRole)
            {
                throw new Exception("You're not a writer user, this action is only for writer users");
            }

            PostDTO oldPost = _postRepository.GetPostById(id);
            if (oldPost.StatusID != _initialStatus)
            {
                throw new Exception("Post cannot be submited");
            }

            using (var scope = new TransactionScope())
            {
                PostDTO post = _postRepository.UpdatePostStatus(id, _pendingStatus, (int)userId);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = post.ID }, post);
            }
        }

        [HttpPut("approve/{id}", Name = "ApprovePost")]
        [Authorize]
        public IActionResult ApprovePost(int id)
        {
            if (roleId != _editorRole)
            {
                throw new Exception("You're not a editor user, this action is only for editor users");
            }

            PostDTO oldPost = _postRepository.GetPostById(id);
            if (oldPost.StatusID != _pendingStatus)
            {
                throw new Exception("Post cannot be approved");
            }

            using (var scope = new TransactionScope())
            {
                PostDTO post = _postRepository.UpdatePostStatus(id, _publishedStatus, (int)userId);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = post.ID }, post);
            }
        }

        [HttpPut("reject/{id}", Name = "RejectPost")]
        [Authorize]
        public IActionResult RejectPost(int id)
        {
            if (roleId != _editorRole)
            {
                throw new Exception("You're not a editor user, this action is only for editor users");
            }

            PostDTO oldPost = _postRepository.GetPostById(id);
            if (oldPost.StatusID != _pendingStatus)
            {
                throw new Exception("Post cannot be rejected");
            }

            using (var scope = new TransactionScope())
            {
                PostDTO post = _postRepository.UpdatePostStatus(id, _rejectedStatus, (int)userId);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = post.ID }, post);
            }
        }

        [HttpDelete("{id}", Name = "DeletePost")]
        [Authorize]
        public IActionResult DeletePost(int id)
        {
            if (roleId != _editorRole)
            {
                throw new Exception("You're not a editor user, this action is only for editor users");
            }

            using (var scope = new TransactionScope())
            {
                PostDTO post = _postRepository.UpdatePostStatus(id, _deletedStatus, (int)userId);
                scope.Complete();
                return new NoContentResult();
            }
        }

        [HttpPost("comment", Name = "CommentPost")]
        public IActionResult CommentPost([FromBody] PostCommentDTO comment)
        {
            PostDTO oldPost = _postRepository.GetPostById(comment.PostID);

            if (oldPost.StatusID != _publishedStatus)
            {
                throw new Exception("Post cannot be commented");
            }

            using (var scope = new TransactionScope())
            {
                PostDTO post = _postRepository.CommentPost(comment);
                scope.Complete();
                return CreatedAtAction(nameof(Get), new { id = post.ID }, post);
            }
        }

    }
}