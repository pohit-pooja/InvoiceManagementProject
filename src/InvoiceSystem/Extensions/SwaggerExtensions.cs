using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public static class SwaggerExtensions
{
    public static void ConfigureForDateOnly(this SwaggerGenOptions options)
    {
        options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });
    }
}
