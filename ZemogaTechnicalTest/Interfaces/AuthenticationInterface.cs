using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.DTO;

namespace ZemogaTechnicalTest.Interfaces
{
    public interface AuthenticationInterface : IDisposable
    {
        AuthenticationDTO Login(int id, string username, string role);
        AuthenticationFrontDTO LoginFront(int id, string username, string userfullname, string role);
        AuthenticationDTO RefreshToken();
    }
}
