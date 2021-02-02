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
    public class IndexModel : PageModel
    {
        private APIConnector _api;
        public IndexModel()
        {
            _api = new APIConnector();
        }

        [BindProperty]
        public LoginDTO LoginModel { get; set; }

        public IActionResult OnGet()
        {
            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    string roleId = ((ClaimsIdentity)User.Identity).FindFirst("role").Value;
                    if (roleId == "1")
                    {
                        return RedirectToPage("OwnPost");
                    }
                    else
                    if (roleId == "2")
                    {
                        return RedirectToPage("PendingPosts");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }

            return this.Page();
        }

        public async Task<IActionResult> OnPostOffline()
        {
            return RedirectToPage("PublishedPost");
        }
        public async Task<IActionResult> OnPostLogIn()
        {
            try
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        
                        AuthenticationDTO loginResponse = JsonConvert.DeserializeObject<AuthenticationDTO>(_api.Login(LoginModel).ToString());
                        await SignInUser(loginResponse, false);
                        if (loginResponse.UserRole == "1")
                        {
                            return RedirectToPage("OwnPost");
                        } else
                        if (loginResponse.UserRole == "2")
                        {
                            return RedirectToPage("PendingPosts");
                        }
                        return RedirectToPage("PublishedPost");
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError(string.Empty, "Invalid username or password.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            
            return Page();
        }

        private async Task SignInUser(AuthenticationDTO loginResponse, bool isPersistent)
        {
            var claims = new List<Claim>();

            try
            {
                claims.Add(new Claim("token", string.Format("Bearer {0}", loginResponse.Token)));
                claims.Add(new Claim("userid", loginResponse.UserID));
                claims.Add(new Claim("username", loginResponse.Username));
                claims.Add(new Claim("userfullname", loginResponse.UserFullname));
                claims.Add(new Claim("role", loginResponse.UserRole));
                var claimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var claimPrincipal = new ClaimsPrincipal(claimIdenties);
                var authenticationManager = Request.HttpContext;

                await authenticationManager.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimPrincipal, new AuthenticationProperties() { IsPersistent = isPersistent });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
