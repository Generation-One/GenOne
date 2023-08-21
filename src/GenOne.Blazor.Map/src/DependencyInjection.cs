using GenOne.DPBlazorMapLibrary.DI;
using Microsoft.Extensions.DependencyInjection;

namespace GenOne.Blazor.Map
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGenOneMaps(this IServiceCollection services, Action<MapOptions>? configureOptions = null)
        {
            if (configureOptions is not null)
                services.PostConfigure(configureOptions);

            return services.AddMapService();
        }

        public static IServiceCollection AddUserLocationProvider<T>(this IServiceCollection services) where T : class, IUserLocationProvider
        {
            return services.AddSingleton<IUserLocationProvider, T>();
        }
    }

    public class MapOptions
    {
        public string Attribution { get; set; } = "&copy; <a href=\"https://www.openstreetmap.org/copyright\">OpenStreetMap</a> contributors";

        public string UrlTileTemplate { get; set; } = "https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png";
    }
}