using Microsoft.Extensions.DependencyInjection;

namespace FileFormatting {
    public static class ServiceInjector {
        public static IServiceCollection AddFileFormatters(this IServiceCollection services) {
            services.AddScoped<IFileFormatter, JsonFormatter>();
            services.AddScoped<IFileFormatter, CsvFormatter>();
            
            return services;
        }
    }
}