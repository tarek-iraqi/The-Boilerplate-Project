using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class IdentityResponseDTO
    {
        public bool success { get; set; }
        public List<Tuple<string, string>> errors { get; set; }
    }
}
