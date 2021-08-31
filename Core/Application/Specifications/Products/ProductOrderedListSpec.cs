using Application.DTOs;
using Domain.Entities;
using Helpers.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Products
{
    public class ProductOrderedListSpec : Specification<Product, ProductResponseDTO>
    {
        public ProductOrderedListSpec()
        {
            Query.Select(a => new ProductResponseDTO
            {
                id = a.Id,
                name = a.Name,
                rate = a.Rate
            });
        }
    }
}
