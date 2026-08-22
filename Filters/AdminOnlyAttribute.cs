using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FinalProject.Filters
{
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(
            ActionExecutingContext context)
        {
            if (context.HttpContext.Session.GetString("IsAdmin") != "true")
            {
                context.Result = new RedirectToActionResult(
                    "AdminLogin",
                    "Home",
                    null
                );
            }
        }
    }
}