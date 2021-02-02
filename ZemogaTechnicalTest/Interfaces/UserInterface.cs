using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.DTO;

namespace ZemogaTechnicalTest.Interfaces
{
    public interface UserInterface
    {
        AuthenticationDTO Login(LoginDTO credentials);
        AuthenticationFrontDTO LoginFront(LoginDTO credentials);
    }
}
