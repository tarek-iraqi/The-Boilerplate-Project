using Application.Contracts;
using Application.DTOs;
using Domain.DomainEvents;
using Helpers.Constants;
using Helpers.Localization;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace Application.DomainEventHandlers
{
    internal class RegisterUserDomainEventHandler : INotificationHandler<RegisterUserDomainEvent>
    {
        private readonly IEmailSender _emailSender;
        private readonly IApplicationLocalization _localizer;

        public RegisterUserDomainEventHandler(IEmailSender emailSender,
            IApplicationLocalization localizer)
        {
            _emailSender = emailSender;
            _localizer = localizer;
        }
        public async Task Handle(RegisterUserDomainEvent notification, CancellationToken cancellationToken)
        {
            var emailModel = new AccountVerificationEmailDTO
            {
                name = notification.Name,
                token = notification.Token
            };

            await _emailSender.SendSingleEmail(notification.Email,
                _localizer.Get(LocalizationKeys.AccountConfirmation),
                KeyValueConstants.AccountVerificationEmailTemplate,
                emailModel);
        }
    }
}
