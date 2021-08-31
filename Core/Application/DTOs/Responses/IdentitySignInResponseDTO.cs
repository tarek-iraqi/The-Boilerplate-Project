using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class IdentitySignInResponseDTO
    {
        public bool success { get; set; }
        public bool isLockedOut { get; set; }
    }
}
