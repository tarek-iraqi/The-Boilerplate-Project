using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApi.Filters
{
    public class ErrorResultHandleAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result;

            base.OnActionExecuted(context);
        }
    }
}
