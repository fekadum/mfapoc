using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace secretpoc
{
    public class RequireSensitiveReauthAttribute : ActionFilterAttribute
    {
        private readonly string _actionKey;

        public RequireSensitiveReauthAttribute(string actionKey)
        {
            _actionKey = actionKey;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var session = context.HttpContext.Session;
            var verifiedAtStr = session.GetString(_actionKey);

            if (string.IsNullOrEmpty(verifiedAtStr) ||
                !DateTime.TryParse(verifiedAtStr, out var verifiedAt) ||
                verifiedAt.AddMinutes(5) < DateTime.UtcNow)
            {
                context.Result = new RedirectToActionResult("VerifyAuthenticator", "Security", new { actionKey = _actionKey });
            }
        }
    }

}
