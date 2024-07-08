using Microsoft.Extensions.Logging;
using ProyectoP2.DataAccess;
using ProyectoP2.ViewModels;
using CommunityToolkit.Maui;
using ProyectoP2.Views;
using Microsoft.EntityFrameworkCore;
using ProyectoP2.DataAccess;
using Microsoft.Extensions.Logging;

namespace ProyectoP2
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-solid-900.ttf", "FaSolid");
                });

            builder.Services.AddDbContext<VentaDbContext>();

            builder.Services.AddTransient<CategoriasPage>();
            builder.Services.AddTransient<CategoriasVM>();

            builder.Services.AddTransient<InventarioPage>();
            builder.Services.AddTransient<InventarioVM>();

            builder.Services.AddTransient<ProductoPage>();
            builder.Services.AddTransient<ProductoVM>();

            builder.Services.AddTransient<VentaPage>();
            builder.Services.AddTransient<VentaVM>();

            builder.Services.AddTransient<BuscarProductoPage>();
            builder.Services.AddTransient<BuscarProductoVM>();

            builder.Services.AddTransient<HistoriaVentaPage>();
            builder.Services.AddTransient<HistorialVentaVM>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainVM>();

            var dbContext = new VentaDbContext();
            dbContext.Database.EnsureCreated();
            dbContext.Dispose();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            Routing.RegisterRoute(nameof(ProductoPage), typeof(ProductoPage));
            Routing.RegisterRoute(nameof(BarcodePage), typeof(BarcodePage));
            Routing.RegisterRoute(nameof(BuscarProductoPage), typeof(BuscarProductoPage));
            
            return builder.Build();
        }
    }
}