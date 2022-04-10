using Application.Authorization;
using Application.Features.ExportSamplePDF;
using Application.Features.UserAccount.Commands;
using Application.Features.UserAccount.Queries;
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
        public async Task<IActionResult> Register(Register.Command command)
        {
            return new JsonResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Verify user account after registeration
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/verify")]
        public async Task<IActionResult> VerifyAccount(VerifyAccount.Command command)
        {
            return new JsonResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Login request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/login")]
        public async Task<IActionResult> Login(Login.Command command)
        {
            command.ip_address = GenerateIPAddress();
            return new JsonResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Forget password request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/password/forget")]
        public async Task<IActionResult> ForgetPassword(ForgetPassword.Command command)
        {
            return new JsonResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Reset password request
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// 
        [AllowAnonymous]
        [HttpPost(baseRoute + "/password/reset")]
        public async Task<IActionResult> ResetPassword(ResetPassword.Command command)
        {
            return new JsonResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(baseRoute)]
        public async Task<IActionResult> UpdateProfile(UpdateProfile.Command command)
        {
            return new JsonResult(await Mediator.Send(command));
        }

        /// <summary>
        /// Update user profile picture
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        [HttpPost(baseRoute + "/profile_picture")]
        public async Task<IActionResult> UpdateProfilePicture([FromForm] IFormFile file)
        {
            return new JsonResult(await Mediator.Send(new UploadProfileImage.Command(file)));
        }

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
        public async Task<IActionResult> GetUsersList([FromQuery] string name,
            [FromQuery] Dictionary<string, string> sort,
            [FromQuery] int page_number, [FromQuery] int page_size)
        {
            var result = await Mediator.Send(new UsersList.Query(name, sort.GetValueOrDefault(SortKey.by),
                sort.GetValueOrDefault(SortKey.order), page_number, page_size));

            return new JsonResult(result);
        }

        /// <summary>
        /// export sample pdf file
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(baseRoute + "/pdf")]
        public async Task<IActionResult> ExportPdfFile()
        {
            var result = await Mediator.Send(new ExportSampleImage.Command());

            return File(result, "application/pdf", "sample.pdf");
        }

        /// <summary>
        /// export sample image file
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(baseRoute + "/image")]
        public async Task<IActionResult> ExportImageFile()
        {
            var result = await Mediator.Send(new ExportSampleImage.Command());

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
