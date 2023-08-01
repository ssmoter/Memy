using Memy.Server.TokenAuthentication;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Memy.Server.Filtres
{
    public class AdminAuthenticationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var adminTokenManagerObj = context.HttpContext.RequestServices.GetService(typeof(IAdminTokenManager));
            IAdminTokenManager? adminTokenManager = null;
            if (adminTokenManagerObj is IAdminTokenManager)
            {
                adminTokenManager = (IAdminTokenManager)adminTokenManagerObj;
            }


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
                if (adminTokenManager is not null)
                {
                    bool CheckToken = adminTokenManager.VerifyToken(token);
                    if (!CheckToken)
                    {
                        result = false;
                    }
                }
            }
            if (!result)
            {
                context.Result = new UnauthorizedObjectResult(new Memy.Shared.Model.Error()
                {
                    Typ = Shared.Helper.MyEnums.TaskName.Error,
                    Message = $"You are not Authorized.{Environment.NewLine}Please login.",
                });
            }
        }
    }
}
