using Microsoft.AspNetCore.Identity;

public class RoleRedirectMiddleware
{
    private readonly RequestDelegate _next;

    public RoleRedirectMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            if (context.Request.Path.StartsWithSegments("/Admin") && !context.User.IsInRole("Admin"))
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }

            if (context.Request.Path.StartsWithSegments("/User") && !context.User.IsInRole("User") && !context.User.IsInRole("Admin"))
            {
                context.Response.Redirect("/Home/AccessDenied");
                return;
            }
        }
        await _next(context);
    }
}
