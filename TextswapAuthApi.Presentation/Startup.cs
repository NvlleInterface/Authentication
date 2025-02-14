using Microsoft.Extensions.Options;
using System.ComponentModel;
using System.Text.Json.Serialization;
using System.Text.Json;
using Asp.Versioning;
using Swashbuckle.AspNetCore.SwaggerGen;
using Asp.Versioning.ApiExplorer;

namespace TextswapAuthApi.Presentation
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true;
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(1, 0);
            }).AddApiExplorer(o =>
            {
                o.GroupNameFormat = "'v'VVV";
                o.SubstituteApiVersionInUrl = true;
            });
            services.AddAutoMapper((serviceProvider, cfg) =>
            {
                //  cfg.AddExpressionMapping();
                // cfg.AddCollectionMappers();
                cfg.AddProfile<PresentationMappingProfile>();
            }, new System.Reflection.Assembly[0]);
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => builder
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .WithOrigins("*")
                .AllowAnyMethod()
                .AllowAnyHeader());
            });

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env, IApiVersionDescriptionProvider provider)
        {

            app.UseCors("CorsPolicy");

            // #if DEBUG
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            //A common endpoint that contains both versions
            app.UseSwaggerUI();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // #endif
        }
    }
}
