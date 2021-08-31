using Application.Features.ProductFeatures.Commands;
using Application.Features.ProductFeatures.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [AllowAnonymous]
    public class ProductsController : BaseApiController
    {
        private const string baseRoute = "products";

        /// <summary>
        /// Create new product
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPost(baseRoute)]
        public async Task<IActionResult> Create(Create.Command command)
        {
            await Mediator.Send(command);
            return Ok();
        }

        /// <summary>
        /// Gets all Products.
        /// </summary>
        /// <returns></returns>
        [HttpGet(baseRoute)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await Mediator.Send(new GetAll.Query()));
        }

        /// <summary>
        /// Get paginated product list
        /// </summary>
        /// <param name="page_number"></param>
        /// <param name="page_size"></param>
        /// <returns></returns>
        [HttpGet(baseRoute + "/list")]
        public async Task<IActionResult> GetPaginatedList([FromQuery]int page_number, [FromQuery] int page_size)
        {
            return Ok(await Mediator.Send(new GetPaginatedList.Query(page_size, page_number)));
        }


        /// <summary>
        /// Gets Product Entity by Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet(baseRoute + "/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return Ok(await Mediator.Send(new GetById.Query(id)));
        }


        /// <summary>
        /// Deletes Product Entity based on Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete(baseRoute + "/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await Mediator.Send(new Delete.Command { id = id });
            return Ok();
        }


        /// <summary>
        /// Update product data
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut(baseRoute + "/{id}")]
        public async Task<IActionResult> Update(int id, Update.Command command)
        {
            command.id = id;
            await Mediator.Send(command);
            return Ok();
        }
    }
}
