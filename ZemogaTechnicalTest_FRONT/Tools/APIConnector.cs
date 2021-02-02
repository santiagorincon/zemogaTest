using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using ZemogaTechnicalTest_FRONT.DTO;

namespace ZemogaTechnicalTest_FRONT.Tools
{
    public partial class APIConnector
    {
        private string urlAPI = "https://localhost:44335/api/";
        private string loginService = "authentication/front";
        private string postService = "post";
        private string commentService = "post/comment";
        private string pendingPostsService = "post/pending";
        private string approvePostsService = "post/approve";
        private string rejectPostsService = "post/reject";
        private string ownPostsService = "post/own";
        private string submitPostsService = "post/submit";

        public APIConnector()
        {

        }

        public object Login(LoginDTO credentials)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}", urlAPI, loginService), null, HttpHelper.RequestType.POST, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null
                , JsonConvert.SerializeObject(credentials));
            return result.token;
        }

        public List<PostDTO> GetPublishedPosts()
        {
            List<PostDTO> result = HttpHelper.CreateNetworkRequest<List<PostDTO>>(
                string.Format("{0}{1}", urlAPI, postService), null, HttpHelper.RequestType.GET, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null);
            return result;
        }

        public PostDTO GetPostById(int postId)
        {
            PostDTO result = HttpHelper.CreateNetworkRequest<PostDTO>(
                string.Format("{0}{1}/{2}", urlAPI, postService, postId), null, HttpHelper.RequestType.GET, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null);
            return result;
        }

        public object SendComment(PostCommentDTO comment)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}", urlAPI, commentService), null, HttpHelper.RequestType.POST, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null
                , JsonConvert.SerializeObject(comment));
            return result.token;
        }

        public object GetPendingPosts(string token)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}", urlAPI, pendingPostsService), token, HttpHelper.RequestType.GET, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null);
            return result;
        }

        public object ApprovePost(int postId, string token)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}/{2}", urlAPI, approvePostsService, postId), token, HttpHelper.RequestType.PUT, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null);
            return result.token;
        }

        public object RejectPost(int postId, string token)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}/{2}", urlAPI, rejectPostsService, postId), token, HttpHelper.RequestType.PUT, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null);
            return result.token;
        }

        public object DeletePost(int postId, string token)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}/{2}", urlAPI, postService, postId), token, HttpHelper.RequestType.DELETE, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null);
            return result.token;
        }

        public object GetOwnPosts(string token)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}", urlAPI, ownPostsService), token, HttpHelper.RequestType.GET, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null);
            return result;
        }

        public object AddPost(string token, PostDTO post)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}", urlAPI, postService), token, HttpHelper.RequestType.POST, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null
                , JsonConvert.SerializeObject(post));
            return result;
        }

        public object EditPost(string token, PostDTO post)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}", urlAPI, postService), token, HttpHelper.RequestType.PUT, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null
                , JsonConvert.SerializeObject(post));
            return result;
        }

        public object SubmitPost(int postId, string token)
        {
            DefaultResponseDTO result = HttpHelper.CreateNetworkRequest<DefaultResponseDTO>(
                string.Format("{0}{1}/{2}", urlAPI, submitPostsService, postId), token, HttpHelper.RequestType.PUT, HttpHelper.ContentType.JSON, HttpHelper.Accept.JSON, null);
            return result.token;
        }
    }
}
