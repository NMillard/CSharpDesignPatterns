using Microsoft.Extensions.DependencyInjection;

/*
 *  This is basically how you'd write the service injection for
 *  registering the file formatters with the DI container.
 */

namespace FileFormatting {
    public static class ServiceInjector {
        public static IServiceCollection AddFileFormatters(this IServiceCollection services) {
            services.AddScoped<ITransactionsFormatter, JsonFormatter>();
            services.AddScoped<ITransactionsFormatter, CsvFormatter>();
            
            return services;
        }
    }
}