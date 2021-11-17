using Application.Interfaces;
using Domain.Entities;
using FluentValidation;
using Helpers.Constants;
using Helpers.Enums;
using Helpers.Exceptions;
using Helpers.Interfaces;
using Helpers.Models;
using Helpers.Resources;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.UserAccount.Commands
{
    public class UploadProfileImage
    {
        public class Command: IRequest<Result<string>>
        {
            public IFormFile file { get; }
            public Command(IFormFile file)
            {
                this.file = file;
            }
        }

        public class CommandValidator: AbstractValidator<Command>
        {
            public CommandValidator(IApplicationLocalization localizer)
            {
                RuleFor(p => p.file).NotEmpty().WithName(localizer.Get(ResourceKeys.File));
            }
        }

        public class Handler : IRequestHandler<Command, Result<string>>
        {
            private readonly IUnitOfWork _uow;
            private readonly IAuthenticatedUserService _authenticatedUserService;
            private readonly IFileValidator _fileValidator;
            private readonly IUpload _upload;

            public Handler(IUnitOfWork uow, IAuthenticatedUserService authenticatedUserService,
                IFileValidator fileValidator, IUpload upload)
            {
                _uow = uow;
                _authenticatedUserService = authenticatedUserService;
                _fileValidator = fileValidator;
                _upload = upload;
            }
            public async Task<Result<string>> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _uow.Repository<AppUser>()
                    .GetByIdAsync(Guid.Parse(_authenticatedUserService.UserId));

                var validationResult = _fileValidator.IsValidFile(request.file, 1,
                    new[] { FileExtensions.jpeg, FileExtensions.png, FileExtensions.jpg });

                if (!validationResult.IsSuccess)
                    throw new AppCustomException(ErrorStatusCodes.InvalidAttribute,
                        new List<Tuple<string, string>> { new Tuple<string, string>(nameof(request.file),
                                    validationResult.Message) });

                var fileName = await _upload.UploadFile(request.file, _authenticatedUserService.UserId);

                user.ChangeProfilePicture(fileName);
                await _uow.CompleteAsync();

                return Result<string>.ValueOf($"{_upload.BaseUrl}/{fileName}");
            }
        }
    }
}
