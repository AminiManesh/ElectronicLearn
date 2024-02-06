using ElectronicLearn.Core.Services.Interfaces;
using ElectronicLearn.DataLayer.Entities.User;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ElectronicLearn.Web.Middlewares
{
    public class AccessRestrictionMiddleware
    {
        private readonly RequestDelegate _next;

        public AccessRestrictionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ICourseService courseService)
        {
            if (context.Request.Path.Value.ToString().ToLower().StartsWith("/courses/courseonline") ||
                context.Request.Path.Value.ToString().ToLower().StartsWith("/courses/episodes"))
            {
                var callingUrl = context.Request.Headers["Referer"].ToString();
                if (!string.IsNullOrEmpty(callingUrl) && callingUrl.StartsWith($"{context.Request.Scheme}://{context.Request.Host.Value}"))
                {
                    await _next(context);

                }
                context.Response.Redirect("/Login");
                return;
            }

            await _next(context);
        }
    }
}
