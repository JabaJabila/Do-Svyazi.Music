using DS.Application.CQRS.MusicUser.Queries;
using MediatR;

namespace DS.Music.WebApi.Middlewares;

public class AuthorizationMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizationMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context, IMediator mediator)
    {
        string? token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (token is not null)
            await AttachUserToContext(context, token, mediator);

        await _next(context);
    }

    private static async Task AttachUserToContext(HttpContext context, string token, IMediator mediator)
    {
        var user = await mediator.Send(new GetUserForAuthorization.GetUserQuery(token));
        context.Items["User"] = user;
    }
}

public static class AuthorizationMiddlewareExtension
{
    public static IApplicationBuilder UseAuthorizationMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<AuthorizationMiddleware>();
    }
}