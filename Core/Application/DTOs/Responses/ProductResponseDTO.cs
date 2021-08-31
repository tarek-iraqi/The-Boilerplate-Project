using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ProductResponseDTO
    {
        public int id { get; set; }
        public string name { get; set; }
        public decimal rate { get; set; }
    }
}
