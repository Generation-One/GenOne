using GenOne.Blazor.BottomSheet.JsInterop;
using Microsoft.Extensions.DependencyInjection;

namespace GenOne.Blazor.BottomSheet.DependencyInjection
{
    public static class BottomSheetDependencyInjection
    {
        public static IServiceCollection AddBottomSheet(this IServiceCollection services)
        {
            services.AddTransient<BottomSheetJsInterop>();
            return services;
        }
    }
}
