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
    public class PendingPostsModel : PageModel
    {
        private APIConnector _api = new APIConnector();
        private string _token;
        [BindProperty]
        public List<PostDTO> pendingPosts { get; set; }
        public void OnGet()
        {
            var authenticationManager = Request.HttpContext;
            _token = ((ClaimsIdentity)User.Identity).FindFirst("token").Value;
            DefaultResponseDTO serviceResponse = (DefaultResponseDTO)(_api.GetPendingPosts(_token));

            pendingPosts = JsonConvert.DeserializeObject<List<PostDTO>>(serviceResponse.response.ToString());
        }

        public async Task<IActionResult> OnPostApprove()
        {
            int id = int.Parse(Request.Form["id"]);
            var authenticationManager = Request.HttpContext;
            _token = ((ClaimsIdentity)User.Identity).FindFirst("token").Value;
            _api.ApprovePost(id, _token);
            return RedirectToPage("PendingPosts");
        }

        public async Task<IActionResult> OnPostReject()
        {
            int id = int.Parse(Request.Form["id"]);
            var authenticationManager = Request.HttpContext;
            _token = ((ClaimsIdentity)User.Identity).FindFirst("token").Value;
            _api.RejectPost(id, _token);
            return RedirectToPage("PendingPosts");
        }

        public async Task<IActionResult> OnPostDelete()
        {
            int id = int.Parse(Request.Form["id"]);
            var authenticationManager = Request.HttpContext;
            _token = ((ClaimsIdentity)User.Identity).FindFirst("token").Value;

            _api.DeletePost(id, _token);
            return RedirectToPage("PendingPosts");
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