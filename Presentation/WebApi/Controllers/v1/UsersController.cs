using Application.Authorization;
using Application.DTOs;
using Application.Features.Commands;
using Application.Features.Queries;
using Helpers.BaseModels;
using Helpers.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    public class UsersController : BaseApiController
    {
        private const string baseRoute = "users";

        /// <summary>
        /// Register new user account
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] Registeration_Command command) =>
            EndpointResult(await _mediator.Send(command));

        /// <summary>
        /// Verify user account after registeration
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/verify")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> VerifyAccount(VerifyAccount_Command command) =>
            EndpointResult(await _mediator.Send(command));

        /// <summary>
        /// Login request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/login")]
        [ProducesResponseType(typeof(Result<LoginResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] Login_Command command)
        {
            command = command with { ip_address = GenerateIPAddress() };

            return EndpointResult(await _mediator.Send(command));
        }

        /// <summary>
        /// Forget password request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/password/forget")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPassword_Command command) =>
            EndpointResult(await _mediator.Send(command));

        /// <summary>
        /// Reset password request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/password/reset")]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword_Command command) =>
            EndpointResult(await _mediator.Send(command));


        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(baseRoute)]
        [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfile_Command command) =>
            EndpointResult(await _mediator.Send(command));

        /// <summary>
        /// Update user profile picture
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost(baseRoute + "/profile_picture")]
        [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] IFormFile file) =>
            EndpointResult(await _mediator.Send(new UploadProfileImage_Command(file)));

        /// <summary>
        /// Get paginated list of users 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sort"></param>
        /// <param name="page_number"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        /// 
        [HasPermission(Permissions.ViewUsers)]
        [HttpGet(baseRoute)]
        [ProducesResponseType(typeof(PaginatedResult<UsersListResponseDTO>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetUsersList([FromQuery] string name,
            [FromQuery] Dictionary<string, string> sort,
            [FromQuery] int page_number, [FromQuery] int page_size)
        {
            var result = await _mediator.Send(new GetUsersList_Query(name, sort.GetValueOrDefault(SortKey.by),
                sort.GetValueOrDefault(SortKey.order), page_number, page_size));

            return EndpointResult(result);
        }

        /// <summary>
        /// export sample pdf file
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(baseRoute + "/pdf")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportPdfFile()
        {
            var result = await _mediator.Send(new ExportSamplePDF_Command());

            return File(result, "application/pdf", "sample.pdf");
        }

        /// <summary>
        /// export sample image file
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(baseRoute + "/image")]
        [ProducesResponseType(typeof(FileContentResult), StatusCodes.Status200OK)]
        public async Task<IActionResult> ExportImageFile()
        {
            var result = await _mediator.Send(new ExportSampleImage_Command());

            return File(result, "image/png", "sample.png");
        }

        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
