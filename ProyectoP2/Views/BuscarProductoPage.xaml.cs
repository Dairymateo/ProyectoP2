using ProyectoP2.ViewModels;
namespace ProyectoP2.Views;

public partial class BuscarProductoPage : ContentPage
{
	public BuscarProductoPage(BuscarProductoVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}