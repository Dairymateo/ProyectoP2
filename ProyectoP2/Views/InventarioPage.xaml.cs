using ProyectoP2.DataAccess;
using ProyectoP2.ViewModels;

namespace ProyectoP2.Views;

public partial class InventarioPage : ContentPage
{
    private InventarioVM _viewModel;
    public InventarioPage(InventarioVM viewModel)
	{
		InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
        
    }

    private async void ActualizarProductos(object sender, EventArgs e)
    {
        if(sender is Button button )
        {
            await _viewModel.Actualizar(); // Llamar al método del ViewModel
        }
    }

}