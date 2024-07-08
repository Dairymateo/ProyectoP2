using ProyectoP2.ViewModels;

namespace ProyectoP2.Views;

public partial class VentaPage : ContentPage
{
	public VentaPage(VentaVM vm)
	{
		InitializeComponent();
        BindingContext = vm;
    }
}