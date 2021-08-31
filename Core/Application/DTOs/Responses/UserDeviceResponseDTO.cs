using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class UserDeviceResponseDTO
    {
        public long id { get; set; }
        public string model { get; set; }
        public string token { get; set; }
    }
}
