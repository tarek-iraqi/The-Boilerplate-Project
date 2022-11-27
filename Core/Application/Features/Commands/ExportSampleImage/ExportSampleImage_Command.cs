using MediatR;

namespace Application.Features.Commands;

public record ExportSampleImage_Command : IRequest<byte[]>;
