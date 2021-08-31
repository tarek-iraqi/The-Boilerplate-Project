using Application.DTOs;
using Domain.Entities;
using Helpers.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Specifications.Devices
{
    public class DevicesFilteredByUserSpec : Specification<Device, UserDeviceResponseDTO>
    {
        public DevicesFilteredByUserSpec(Guid userId)
        {
            Query.Where(device => device.UserId == userId)
                 .OrderByDescending(device => device.CreatedOn);
            Query.Select(device => new UserDeviceResponseDTO
            {
                id = device.Id,
                model = device.Model,
                token = device.Token
            });
        }
    }
}
