using Application.Features.RoleManagement.Commands;
using Application.Features.RoleManagement.Queries;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
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
        public async Task<IActionResult> GetRolesList([FromQuery] int page_number, [FromQuery] int page_size)
        {
            var result = await Mediator.Send(new GetRolesWithPermissions.Query(page_size, page_number));

            return new JsonResult(result);
        }

        /// <summary>
        /// create new role with optional permissions
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(baseRoute)]
        public async Task<IActionResult> CreateRole(CreateRole.Command command)
        {
            var result = await Mediator.Send(command);

            return new JsonResult(result);
        }
    }
}
