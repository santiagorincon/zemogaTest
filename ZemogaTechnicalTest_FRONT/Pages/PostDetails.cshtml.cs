using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ZemogaTechnicalTest_FRONT.DTO;
using ZemogaTechnicalTest_FRONT.Tools;

namespace ZemogaTechnicalTest_FRONT.Pages
{
    public class PostDetailsModel : PageModel
    {
        private APIConnector _api = new APIConnector();

        [BindProperty(SupportsGet = true)]
        public PostDTO post { get; set; }

        [BindProperty(SupportsGet = true)]
        public int postId { get; set; }
        public void OnGet()
        {
            post = _api.GetPostById(postId);
        }

        public async Task<IActionResult> OnPostSendComment()
        {
            string comment = Request.Form["comment"];
            int? userId = null;
            if(User.Identity.IsAuthenticated)
            {
                userId = int.Parse(((ClaimsIdentity)User.Identity).FindFirst("userid").Value);
            }

            PostCommentDTO newComment = new PostCommentDTO
            {
                Comment = comment,
                PostID = postId,
                UserID = userId
            };

            _api.SendComment(newComment);
            return RedirectToAction("PostDetails", new { postId });
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