using ProyectoP2.DTOs;
using ProyectoP2.ViewModels;

namespace ProyectoP2.Views;

public partial class HistoriaVentaPage : ContentPage
{
    private HistorialVentaVM _viewModel;

    public HistoriaVentaPage(HistorialVentaVM vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }

    private async void DescargarVenta(object sender, EventArgs e)
    {
        if (sender is Button button && button.BindingContext is VentaDTO venta)
        {
            await _viewModel.DescargarVentaAsync(venta); // Llamar al método del ViewModel
        }
    }

    private async void ActualizarProductos(object sender, EventArgs e)
    {
        if (sender is Button button)
        {
            await _viewModel.Actualizar(); // Llamar al método del ViewModel
        }
    }


}
