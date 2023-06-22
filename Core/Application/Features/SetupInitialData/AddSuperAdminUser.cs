using Application.Authorization;
using Application.Contracts;
using Domain.Entities;
using Domain.ValueObjects;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.SetupInitialData;

public class AddSuperAdminUser
{
    public class Command : IRequest
    {

    }

    private class Handler : IRequestHandler<Command>
    {
        private readonly IIdentityService _identityService;

        public Handler(IIdentityService identityService)
        {
            _identityService = identityService;
        }

        public async Task Handle(Command request, CancellationToken cancellationToken)
        {
            var user = await _identityService.FindByEmail("super@admin.com");

            if (user == null)
            {
                var name = Name.Create("super", "admin");
                user = new AppUser(name, "super@admin.com", "super@admin.com", isEmailConfirmed: true);

                await _identityService.Add(user, "super@dm1n");

                var role = AppRole.Create("Super Admin", DefaultRoles.SUPER_ADMIN.ToString());

                await _identityService.AddNewRole(role);

                await _identityService.AddUserToRole(user, role.Alias);
            }
        }
    }
}