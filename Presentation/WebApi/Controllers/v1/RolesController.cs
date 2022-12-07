using Application.Features.Commands;
using Application.Features.Queries;
using Application.Features.Queries.GetRolesWithPermissions;
using Helpers.BaseModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1;

public class RolesController : BaseApiController
{
    private const string baseRoute = "roles";

    /// <summary>
    /// get list of roles with their permissions if exist
    /// </summary>
    /// <param name="page_number"></param>
    /// <param name="page_size"></param>
    /// <returns></returns>
    [HttpGet(baseRoute)]
    [ProducesResponseType(typeof(PaginatedResult<RolesWithPermissionsDTO>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetRolesList([FromQuery] int page_number, [FromQuery] int page_size) =>
        EndpointResult(await _mediator.Send(new GetRolesWithPermissions_Query(page_size, page_number)));

    /// <summary>
    /// create new role with optional permissions
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost(baseRoute)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateRole(CreateRole_Command command) =>
        EndpointResult(await _mediator.Send(command));
}