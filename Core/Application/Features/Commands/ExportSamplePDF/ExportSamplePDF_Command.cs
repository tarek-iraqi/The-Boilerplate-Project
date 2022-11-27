using MediatR;

namespace Application.Features.Commands;

public record ExportSamplePDF_Command : IRequest<byte[]>;
