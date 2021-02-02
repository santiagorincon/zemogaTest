using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZemogaTechnicalTest.Models
{
    public class Role
    {
        public int ID { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string RoleDesc { get; set; }

        public List<User> Users { get; set; }
    }
}
