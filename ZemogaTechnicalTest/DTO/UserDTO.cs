using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.Models;

namespace ZemogaTechnicalTest.DTO
{
    public class UserDTO
    {
        public UserDTO()
        {

        }

        // This constructor is added for initialize an DTO object with a DB object
        public UserDTO(User user)
        {
            ID = user.ID;
            UserFullname = user.UserFullname;
            Username = user.Username;
            Password = user.Password;
            RoleID = user.RoleID;
            RoleName = user.Role != null ? user.Role.RoleName : string.Empty;
            CreatedDate = user.CreatedDate;
        }

        //Attributes for UserDTO. RoleName was added for return the Role name
        public int ID { get; set; }
        public string UserFullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleID { get; set; }
        public string RoleName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
