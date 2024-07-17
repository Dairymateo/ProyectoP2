using ProyectoP2.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ProyectoP2.Services;

namespace ProyectoP2.Views
{
    public partial class Api : ContentPage
    {
        public Api()
        {
            InitializeComponent();
            // Obtener ApiViewModel desde el contenedor de servicios
            var viewModel = ServiceHelper.ServiceProvider.GetRequiredService<ApiViewModel>();
            this.BindingContext = viewModel;
        }
    }
}
