using Domain.Common;

namespace Domain.DomainEvents
{
    public record RegisterUserDomainEvent(string Name, string Email, string Token) : IDomainEvent
    {
    }
}
