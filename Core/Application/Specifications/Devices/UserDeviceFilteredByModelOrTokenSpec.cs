using Domain.Entities;
using Helpers.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Devices
{
    public class UserDeviceFilteredByModelOrTokenSpec : Specification<Device>
    {
        public UserDeviceFilteredByModelOrTokenSpec(Guid userId, string model, string token)
        {
            Query.Where(device => device.UserId == userId &&
                                  (device.Model == model || device.Token == token));
        }
    }
}
