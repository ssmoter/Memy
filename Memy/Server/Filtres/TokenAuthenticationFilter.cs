using Memy.Server.TokenAuthentication;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Memy.Server.Filtres
{
    public class TokenAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public async void OnAuthorization(AuthorizationFilterContext context)
        {
            ITokenManager? tokenManager = (ITokenManager)context.HttpContext.RequestServices.GetService(typeof(ITokenManager));

            bool result = true;
            if (!context.HttpContext.Request.Headers.ContainsKey(Shared.Helper.Headers.Authorization))
            {
                result = false;
            }

            Guid token;

            if (result)
            {
                if (!Guid.TryParse(context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == Shared.Helper.Headers.Authorization).Value, out token))
                {
                    result = false;
                }
                bool CheckToken = tokenManager.VerifyToken(token);
                if (!CheckToken)
                {
                    result = false;
                }
            }
            if (!result)
            {
                // context.ModelState.AddModelError("Unauthorization", $"You are not Authorized.{Environment.NewLine}Please login.");
                // context.Result = new UnauthorizedObjectResult(context.ModelState);
                context.Result = new UnauthorizedObjectResult(new Memy.Shared.Model.Error()
                {
                    Typ = Shared.Helper.MyEnums.TaskName.Error,
                    Message = $"You are not Authorized.{Environment.NewLine}Please login.",
                });

            }
        }
    }
}
