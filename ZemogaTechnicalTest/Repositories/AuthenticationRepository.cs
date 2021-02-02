using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ZemogaTechnicalTest.DTO;
using ZemogaTechnicalTest.Interfaces;
using ZemogaTechnicalTest.Tools;

namespace ZemogaTechnicalTest.Repositories
{
    public class AuthenticationRepository : AuthenticationInterface
    {
        bool disposed = false;
        public IOptions<JwtAuthentication> _jwtAuthentication;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        public AuthenticationRepository(IHttpContextAccessor httpContextAccessor, IOptions<JwtAuthentication> auth, IConfiguration configuration)
        {
            _jwtAuthentication = auth;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public AuthenticationDTO Login(int id, string username, string role)
        {
            return new AuthenticationDTO
            {
                AuthenticationType = "Bearer",
                Token = GenerateToken(id, username, role)
            };
        }

        public AuthenticationFrontDTO LoginFront(int id, string username, string userfullname, string role)
        {
            return new AuthenticationFrontDTO
            {
                AuthenticationType = "Bearer",
                Token = GenerateToken(id, username, role),
                UserID = id.ToString(),
                Username = username,
                UserFullname = userfullname,
                UserRole = role
            };
        }
        public AuthenticationDTO RefreshToken()
        {
            int unencryptedId = Int32.Parse(Encryptor.Decrypt(((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).Claims.FirstOrDefault(s => s.Type == "userid")?.Value, _configuration["privateKey"]));
            string username = ((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).Claims.FirstOrDefault(s => s.Type == "username")?.Value;
            string role = ((ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity).Claims.FirstOrDefault(s => s.Type == "userrole")?.Value;
            return new AuthenticationDTO
            {
                AuthenticationType = "Bearer",
                Token = GenerateToken(unencryptedId, username, role)
            };
        }

        private string GenerateToken(int id, string username, string role)
        {
            return _jwtAuthentication.Value.GenerateToken(new List<Claim> {
                new Claim("username", username),
                new Claim("userid", Encryptor.Encrypt(id.ToString(),_configuration["PrivateKey"])),
                new Claim("userrole", role)
            });
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            disposed = true;
        }
    }
}
