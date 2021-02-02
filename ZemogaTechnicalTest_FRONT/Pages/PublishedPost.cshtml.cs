using System;
using System.Collections.Generic;
using System.Linq;
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
    public class PublishedPostModel : PageModel
    {
        private APIConnector _api;

        [BindProperty]
        public List<PostDTO> publishedPosts { get; set; }

        public void OnGet()
        {
            _api = new APIConnector();
            publishedPosts = _api.GetPublishedPosts();
        }

        public async Task<IActionResult> OnPostViewDetails(int id)
        {
            return RedirectToPage("PostDetails", new { postId = id});
        }

        public async Task<IActionResult> OnPostGoIndex()
        {
            return RedirectToPage("Index");
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