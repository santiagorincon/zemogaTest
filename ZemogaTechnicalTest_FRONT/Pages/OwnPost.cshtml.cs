using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using ZemogaTechnicalTest_FRONT.DTO;
using ZemogaTechnicalTest_FRONT.Tools;

namespace ZemogaTechnicalTest_FRONT.Pages
{
    public class OwnPostModel : PageModel
    {
        private APIConnector _api = new APIConnector();
        private string _token;
        [BindProperty]
        public List<PostDTO> ownPosts { get; set; }
        public void OnGet()
        {
            var authenticationManager = Request.HttpContext;
            _token = ((ClaimsIdentity)User.Identity).FindFirst("token").Value;
            DefaultResponseDTO serviceResponse = (DefaultResponseDTO)(_api.GetOwnPosts(_token));

            ownPosts = JsonConvert.DeserializeObject<List<PostDTO>>(serviceResponse.response.ToString());
        }

        public async Task<IActionResult> OnPostEditPost()
        {
            string title = Request.Form["title"];
            string content = Request.Form["content"];
            int userId = int.Parse(((ClaimsIdentity)User.Identity).FindFirst("userid").Value);
            int? postId = null;

            if (!string.IsNullOrEmpty(Request.Form["postId"])) {
                postId = int.Parse(Request.Form["postId"]);
            }
              
            PostDTO newPost = new PostDTO
            {
                AuthorID = userId,
                PostName = title, 
                PostContent = content,
                ID = postId
            };

            var authenticationManager = Request.HttpContext;
            _token = ((ClaimsIdentity)User.Identity).FindFirst("token").Value;
            if (postId == null)
            {
                _api.AddPost(_token, newPost);
            } else
            {
                _api.EditPost(_token, newPost);
            }

            return this.RedirectToPage("OwnPost");
        }

        public async Task<IActionResult> OnPostSubmit()
        {
            int id = int.Parse(Request.Form["id"]);
            var authenticationManager = Request.HttpContext;
            _token = ((ClaimsIdentity)User.Identity).FindFirst("token").Value;
            _api.SubmitPost(id, _token);
            return RedirectToPage("OwnPost");
        }

        public async Task<IActionResult> OnPostViewPosts()
        {
            return RedirectToPage("PublishedPost");
        }

        public async Task<IActionResult> OnPostLogOff()
        {
            try
            {
                // Setting.  
                var authenticationManager = Request.HttpContext;

                // Sign Out.  
                await authenticationManager.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                // Info  
                throw ex;
            }

            // Info.  
            return this.RedirectToPage("/Index");
        }
    }
}