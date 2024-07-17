using ProyectoP2.Models;
using ProyectoP2.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace ProyectoP2.ViewModels
{
    public partial class ApiViewModel : ObservableObject
    {
        private readonly BookServices _bookServices;

        // Constructor por defecto requerido por el XAML
        public ApiViewModel()
        {
        }

        // Constructor utilizado por el contenedor de servicios
        public ApiViewModel(BookServices bookServices)
        {
            _bookServices = bookServices;
            ListaProducto = new ObservableCollection<Producto>();
            ObtenerProductoAsync();
            EjecutarBusquedaCommand = new AsyncRelayCommand(ObtenerProductoAsync);
            GuardarLibroCommand = new AsyncRelayCommand(GuardarProductoAsync);
        }

        [ObservableProperty]
        private ObservableCollection<Producto> listaProducto;

        [ObservableProperty]
        private Producto productoSeleccionado;

        [ObservableProperty]
        private bool loadingEsVisible;

        public IAsyncRelayCommand EjecutarBusquedaCommand { get; }
        public IAsyncRelayCommand GuardarLibroCommand { get; }

        private async Task ObtenerProductoAsync()
        {
            LoadingEsVisible = true;

            var producto = await _bookServices.DevuelveProductoAsync();
            if (producto != null)
            {
                ListaProducto.Add(producto);
            }
            LoadingEsVisible = false;
        }

        private async Task GuardarProductoAsync()
        {
            LoadingEsVisible = true;
            await _bookServices.GuardarLibroAsync();
            LoadingEsVisible = false;
        }
    }
}
