using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZemogaTechnicalTest.Models;

namespace ZemogaTechnicalTest.DTO
{
    public class StatusDTO
    {
        public StatusDTO()
        {

        }

        // This constructor is added for initialize an DTO object with a DB object
        public StatusDTO(Status status)
        {

        }

        //Attributes for StatusDTO.
        public int ID { get; set; }
        public string StatusCode { get; set; }
        public string StatusName { get; set; }
        public string StatusDesc { get; set; }
    }
}
