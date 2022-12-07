using Domain.Entities;
using Helpers.BaseModels;
using System;
using System.Linq;

namespace Application.Specifications.Devices;

public class UserDeviceFilteredByModelOrTokenSpec : Specification<Device>
{
    public UserDeviceFilteredByModelOrTokenSpec(Guid userId, string model, string token)
    {
        Query.Where(device => device.UserId == userId &&
                              (device.Model == model || device.Token == token));
    }
}