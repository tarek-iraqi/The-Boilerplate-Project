using Domain.Common;
using Helpers.Constants;
using Helpers.Exceptions;
using Helpers.Resources;
using System;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class Product : Entity<int>
    {
        public string Name { get; private set; }
        public string Barcode { get; private set; }
        public string Description { get; private set; }
        public decimal Rate { get; private set; }

        public void Create(string name, string description, string barecode, decimal rate)
        {
            if (rate < 1 || rate > 10)
                throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                    new List<Tuple<string, string>> { new Tuple<string, string>(ResourceKeys.Rate, ResourceKeys.InvalidRateValue) },
            new Dictionary<int, string[]> { { 0, new string[] { "1", "10" } } });

            Name = name;
            Barcode = barecode;
            Description = description;
            Rate = rate;
        }
        public void Update(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
