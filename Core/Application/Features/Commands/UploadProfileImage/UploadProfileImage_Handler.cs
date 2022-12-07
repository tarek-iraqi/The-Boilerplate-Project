using Application.Contracts;
using Domain.Entities;
using Helpers.Abstractions;
using Helpers.BaseModels;
using Helpers.Constants;
using Helpers.Enums;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Commands;

internal class UploadProfileImage_Handler : ICommandHandler<UploadProfileImage_Command, OperationResult<string>>
{
    private readonly IUnitOfWork _uow;
    private readonly IAuthenticatedUserService _authenticatedUserService;
    private readonly IFileValidator _fileValidator;
    private readonly IUpload _upload;

    public UploadProfileImage_Handler(IUnitOfWork uow, IAuthenticatedUserService authenticatedUserService,
        IFileValidator fileValidator, IUpload upload)
    {
        _uow = uow;
        _authenticatedUserService = authenticatedUserService;
        _fileValidator = fileValidator;
        _upload = upload;
    }
    public async Task<OperationResult<string>> Handle(UploadProfileImage_Command request, CancellationToken cancellationToken)
    {
        var user = await _uow.Repository<AppUser>()
                    .GetByIdAsync(Guid.Parse(_authenticatedUserService.UserId));

        var validationResult = _fileValidator.IsValidFile(request.file, 1,
            new[] { FileExtensions.jpeg, FileExtensions.png, FileExtensions.jpg });

        if (!validationResult.IsSuccess)
            return OperationResult.Fail<string>(ErrorStatusCodes.BadRequest,
                errors: OperationError.Add(nameof(request.file), validationResult.Message));

        var fileName = await _upload.UploadFile(request.file, _authenticatedUserService.UserId);

        user.ChangeProfilePicture(fileName);
        await _uow.CompleteAsync();

        return OperationResult.Success<string>($"{_upload.BaseUrl}/{fileName}");
    }
}