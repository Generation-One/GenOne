using Microsoft.Extensions.DependencyInjection;

namespace GenOne.Blazor.QrScanner
{
    /// <summary>
    /// Dependency injection for qr scanner
    /// </summary>
    public static class QrScannerDependencyInjection
    {
        public static IServiceCollection AddQrScanner(this IServiceCollection services)
        {
            services.AddTransient<QrScannerJsInterop>();
            return services;
        }
    }
}
