using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UsersListResponseDTO
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string mobile_number { get; set; }
    }
}
