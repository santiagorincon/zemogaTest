using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest.DTO
{
    public class AuthenticationFrontDTO
    {
        public string AuthenticationType { get; set; }
        public string Token { get; set; }
        public string UserID { get; set; }
        public string Username { get; set; }
        public string UserFullname { get; set; }
        public string UserRole { get; set; }
    }
}
