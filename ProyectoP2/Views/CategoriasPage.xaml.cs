using ProyectoP2.ViewModels;

namespace ProyectoP2.Views;

public partial class CategoriasPage : ContentPage
{
	public CategoriasPage(CategoriasVM viewmodel)
	{
		InitializeComponent();
        BindingContext = viewmodel;
    }
}