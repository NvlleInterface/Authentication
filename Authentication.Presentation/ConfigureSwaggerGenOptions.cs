using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Authentication.Presentation
{
    public class ConfigureSwaggerGenOptions
    {
        public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
        {
            readonly IApiVersionDescriptionProvider provider;

            public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) =>
                this.provider = provider;

            public void Configure(SwaggerGenOptions options)
            {
                options.DescribeAllParametersInCamelCase();
                options.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" });

                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        $"{description.GroupName}",
                        CreateVersionInfo(description));
                }

                options.DocInclusionPredicate((documentName, apiDescription) =>
                {
                    var version = apiDescription.GroupName;
                    return documentName == version;
                }
                );

                //options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

                options.DocumentFilter<GenerateExtraSchema>();
            }
            private OpenApiInfo CreateVersionInfo(ApiVersionDescription desc)
            {
                var info = new OpenApiInfo()
                {
                    Title = $"AUthentication API v{desc.ApiVersion}",
                    Version = desc.ApiVersion.ToString()
                };

                if (desc.IsDeprecated)
                {
                    info.Description += " This API version has been deprecated. Please use one of the new APIs available from the explorer.";
                }

                return info;
            }
        }

        public class RemoveVersionFromParameter : IOperationFilter
        {
            public void Apply(OpenApiOperation operation, OperationFilterContext context)
            {
                // Ensure operation parameters are not null before processing
                if (operation.Parameters == null)
                {
                    return;
                }

                // Find and remove the "version" parameter if it exists
                var versionParameter = operation.Parameters.SingleOrDefault(p => p.Name == "version");
                if (versionParameter != null)
                {
                    operation.Parameters.Remove(versionParameter);
                }
            }
        }

        public class ReplaceVersionWithExactValueInPath : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
                var paths = new OpenApiPaths();
                foreach (var path in swaggerDoc.Paths)
                {
                    paths.Add(path.Key.Replace("{version}", swaggerDoc.Info.Version), path.Value);
                }
                swaggerDoc.Paths = paths;
            }
        }
        public class GenerateExtraSchema : IDocumentFilter
        {
            public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
            {
            }
        }
    }
}
