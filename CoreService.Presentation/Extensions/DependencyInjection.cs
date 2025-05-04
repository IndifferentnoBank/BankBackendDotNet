namespace CoreService.Presentation.Extensions;

public static class DependencyInjection
{
    public static void UseUnstableMiddleware(this IApplicationBuilder builder)
    {
        //return builder.UseMiddleware<UnstableMiddleware>();
    }
}