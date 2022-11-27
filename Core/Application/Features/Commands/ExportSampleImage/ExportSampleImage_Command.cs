using Helpers.Abstractions;

namespace Application.Features.Commands;

public record ExportSampleImage_Command : ICommand<byte[]>;
