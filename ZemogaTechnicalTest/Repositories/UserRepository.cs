using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.Data;
using ZemogaTechnicalTest.DTO;
using ZemogaTechnicalTest.Interfaces;

namespace ZemogaTechnicalTest.Repositories
{
    public class UserRepository : UserInterface
    {
        private readonly ZemogaContext _dbContext;
        private readonly AuthenticationInterface _authRepository;
        public UserRepository(ZemogaContext dbContext, AuthenticationInterface authRepository)
        {
            _dbContext = dbContext;
            _authRepository = authRepository;
        }
        public AuthenticationDTO Login(LoginDTO credentials)
        {
            UserDTO user = _dbContext.Users.Where(u => u.Username == credentials.login && u.Password == credentials.password).Select(u => new UserDTO(u)).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            return _authRepository.Login(user.ID, user.Username, user.RoleID.ToString());
        }

        public AuthenticationFrontDTO LoginFront(LoginDTO credentials)
        {
            UserDTO user = _dbContext.Users.Where(u => u.Username == credentials.login && u.Password == credentials.password).Select(u => new UserDTO(u)).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            return _authRepository.LoginFront(user.ID, user.Username, user.UserFullname, user.RoleID.ToString());
        }
    }
}
