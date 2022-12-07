using Application.DTOs;
using Application.Features.Commands;
using Application.Features.Queries;
using Helpers.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1;

public class UserDevicesController : BaseApiController
{
    private const string baseRoute = "fcm-tokens";

    /// <summary>
    /// Add / update user device
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut(baseRoute)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AddUpdateDevice([FromBody] AddUpdateUserDevice_Command command) =>
        EndpointResult(await _mediator.Send(command));


    /// <summary>
    /// Get paginated list of user devices
    /// </summary>
    /// <param name="page_number"></param>
    /// <param name="page_size"></param>
    /// <returns></returns>
    [HttpGet(baseRoute)]
    [ProducesResponseType(typeof(PaginatedResult<UserDeviceResponseDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetUserDevices([FromQuery] int page_number, [FromQuery] int page_size) =>
        EndpointResult(await _mediator.Send(new GetUserDevices_Query(page_size, page_number)));
}