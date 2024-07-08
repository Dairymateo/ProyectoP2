using ProyectoP2.ViewModels;

namespace ProyectoP2.Views;

public partial class About : ContentPage
{
	public About()
	{
		InitializeComponent();
        BindingContext = new AboutViewModel();
    }
}