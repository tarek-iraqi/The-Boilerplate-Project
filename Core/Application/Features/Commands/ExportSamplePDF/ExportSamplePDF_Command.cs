using Helpers.Abstractions;

namespace Application.Features.Commands;

public record ExportSamplePDF_Command : ICommand<byte[]>;
