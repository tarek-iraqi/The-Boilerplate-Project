using Application.DTOs;
using Domain.Entities;
using Helpers.BaseModels;
using Helpers.Constants;
using System.Linq;

namespace Application.Specifications.Users;

public class UsersFilteredAndOrderedSpec : Specification<AppUser, UsersListResponseDTO>
{
    public UsersFilteredAndOrderedSpec(string name, string sortBy, string sortOrder)
    {
        Query.Where(user => string.IsNullOrWhiteSpace(name) ||
                            user.Name.First.Contains(name) ||
                            user.Name.Last.Contains(name))
            .Include(user => user.UserRoles)
            .Include(user => user.Logins)
            .Include(user => user.Name);

        switch (sortBy)
        {
            case SortBy.name:
                if (sortOrder == SortOrder.desc)
                    Query.OrderByDescending(user => user.Name.First).ThenByDescending(user => user.Name.Last);
                else
                    Query.OrderBy(user => user.Name.First).ThenBy(user => user.Name.Last);
                break;
            default:
                Query.OrderBy(user => user.Name.First).ThenBy(user => user.Name.Last);
                break;
        }

        Query.Select(user => new UsersListResponseDTO
        {
            id = user.Id.ToString(),
            name = $"{user.Name.First} {user.Name.Last}",
            email = user.Email,
            mobile_number = user.PhoneNumber
        });
    }
}