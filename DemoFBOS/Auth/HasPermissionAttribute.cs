using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace DemoFBOS.Auth
{
	public class HasPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
	{
		private readonly string _permission;

		public HasPermissionAttribute(string permission)
		{
			_permission = permission;
		}

		public void OnAuthorization(AuthorizationFilterContext context)
		{
			if (context.HttpContext.User.Identity?.IsAuthenticated != true)
			{
				context.Result = new UnauthorizedResult();
				return;
			}

			var hasPermission = context.HttpContext.User.HasClaim("Permission", _permission);
			if (!hasPermission)
			{
				context.Result = new ForbidResult();
			}
		}
	}
}
