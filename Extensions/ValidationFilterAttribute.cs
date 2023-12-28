using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using TestAuthenAndTextMessage.Models.DTO;

namespace TestAuthenAndTextMessage.Extensions
{
    public class ValidationFilterAttribute: ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var modelState = context.ModelState;
            if (!modelState.IsValid)
            {
                var errors = modelState.Values.Select(x => x.Errors.Select(y => y.ErrorMessage).FirstOrDefault()).ToList();
                context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                context.Result = new JsonResult(new ResponseModel
                {
                    Error = true,
                    Message = string.Join(", ", errors)
                });
            }
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
