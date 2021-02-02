using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.Models;

namespace ZemogaTechnicalTest.DTO
{
    public class RoleDTO
    {
        public RoleDTO()
        {

        }

        // This constructor is added for initialize an DTO object with a DB object
        public RoleDTO(Role role)
        {
            ID = role.ID;
            RoleCode = role.RoleCode;
            RoleName = role.RoleName;
            RoleDesc = role.RoleDesc;
        }

        //Attributes for RoleDTO.
        public int ID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }
    }
}
