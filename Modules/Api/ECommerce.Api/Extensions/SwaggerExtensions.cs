using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ECommerce.Api.Extensions;

public static class SwaggerRegisterExtensions
{
    private static string _title = "MyBookshop API";
    private static string _description = "Simple E-commerce";

    public static void AddSwaggerModule(this IServiceCollection services, string title = null, string description = null)
    {
        if (!string.IsNullOrWhiteSpace(title))
            _title = title;

        if (!string.IsNullOrWhiteSpace(description))
            _description = description;

        services.AddSwaggerGen(opt =>
        {
            opt.CustomSchemaIds(s => s.FullName?.Replace("+", "."));
            opt.ParameterFilter<LowercaseParameterFilter>();
            opt.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
        });

        services.ConfigureOptions<NamedSwaggerGenOptions>();

        services.AddApiVersioning(opt =>
        {
            opt.DefaultApiVersion = new ApiVersion(1);
            opt.ReportApiVersions = true;
            opt.AssumeDefaultVersionWhenUnspecified = true;
            opt.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
        }).AddApiExplorer(opt =>
        {
            opt.GroupNameFormat = "'v'V";
            opt.SubstituteApiVersionInUrl = true;
        });
    }

    internal static void UseSwaggerModule(this WebApplication app)
    {
        app.UseSwagger();

        app.UseSwaggerUI(options =>
        {
            options.DefaultModelsExpandDepth(-1);

            foreach (var description in app.DescribeApiVersions())
            {
                var url = $"/swagger/{description.GroupName}/swagger.json";
                options.SwaggerEndpoint(url, description.GroupName.ToUpperInvariant());
            }
        });
    }

    private class NamedSwaggerGenOptions(IApiVersionDescriptionProvider provider)
        : IConfigureNamedOptions<SwaggerGenOptions>
    {
        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(
                    description.GroupName,
                    CreateVersionInfo(description));
            }
        }

        private static OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo
            {
                Title = _title,
                Version = description.ApiVersion.ToString(),
                Description = _description
            };

            if (description.IsDeprecated)
            {
                info.Description += "This API version has been deprecated.";
            }

            return info;
        }
    }

    public class LowercaseParameterFilter : IParameterFilter
    {
        public void Apply(OpenApiParameter parameter, ParameterFilterContext context)
            => parameter.Name = char.ToLowerInvariant(parameter.Name[0]) + parameter.Name[1..];
    }
}