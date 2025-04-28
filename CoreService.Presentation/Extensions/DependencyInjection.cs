namespace CoreService.Presentation.Extensions;

public static class DependencyInjection
{
    public static IApplicationBuilder UseUnstableMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<UnstableMiddleware>();
    }
}